using UnityEngine;

/// <summary>
/// Yajang Defense PoC — GameManager.
/// Owns the PoC scene: draws the path and placement cell, spawns one enemy
/// (a Japgwi) after a short delay, and shows a status line.
/// No win/lose logic in the PoC — the goal is only to prove the cloud-build
/// pipeline and the click-to-place + waypoint-movement loop.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("PoC wiring (set in Main.unity)")]
    public Transform[] waypoints;
    public Transform spawnPoint;
    public Transform placementCell;
    public float enemySpeed = 1.2f;
    public float autoStartDelay = 1f;

    private bool _spawned;
    private string _status = "Starting...";

    void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            _status = "ERROR: waypoints missing (need at least 2).";
            Debug.LogError("[Yajang PoC] " + _status);
            return;
        }

        PocVisuals.DrawPath(waypoints);
        if (placementCell != null)
        {
            PocVisuals.DrawCell(placementCell.position);
        }

        _status = "Click the green cell to place the Tteokbokki stall.";
        Invoke(nameof(SpawnEnemy), autoStartDelay);
    }

    void SpawnEnemy()
    {
        if (_spawned) return;
        _spawned = true;

        Vector3 start = spawnPoint != null ? spawnPoint.position : waypoints[0].position;
        GameObject enemy = new GameObject("Enemy_Japgwi");
        enemy.transform.position = start;

        EnemyMover mover = enemy.AddComponent<EnemyMover>();
        mover.waypoints = waypoints;
        mover.speed = enemySpeed;

        PocVisuals.DrawEnemy(enemy.transform);
        _status = "Enemy is walking the path. Click the green cell to place the stall.";
        Debug.Log("[Yajang PoC] Enemy spawned at " + start);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(12, 12, 900, 30), "Yajang Defense PoC — Hanyang night market");
        GUI.Label(new Rect(12, 36, 900, 30), _status);
        GUI.Label(new Rect(12, 60, 900, 30), "Stall: Tteokbokki (1)  |  Enemy: Japgwi (1)  |  Path: 6 waypoints");
    }
}
