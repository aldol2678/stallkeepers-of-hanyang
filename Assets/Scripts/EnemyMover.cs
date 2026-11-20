using UnityEngine;

/// <summary>
/// Yajang Defense PoC — moves one enemy along the waypoint path.
/// Full game: 6 enemy types + boss; PoC: 1 Japgwi, walk-only, no combat.
/// </summary>
public class EnemyMover : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 1.2f;

    public int CurrentWaypoint { get; private set; }
    public bool Finished { get; private set; }

    void Update()
    {
        if (Finished) return;
        if (waypoints == null || waypoints.Length == 0) return;
        if (CurrentWaypoint >= waypoints.Length)
        {
            Finished = true;
            return;
        }

        Transform target = waypoints[CurrentWaypoint];
        if (target == null)
        {
            CurrentWaypoint++;
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            CurrentWaypoint++;
            if (CurrentWaypoint >= waypoints.Length)
            {
                Finished = true;
                Debug.Log("[Yajang PoC] Enemy reached the market centre.");
            }
        }
    }
}
