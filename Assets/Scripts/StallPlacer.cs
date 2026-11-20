using UnityEngine;

/// <summary>
/// Yajang Defense PoC — click-to-place for the single placement cell.
/// Requires a 2D collider on the same GameObject (BoxCollider2D in Main.unity).
/// Full game: 18 cells, 8 stall cards, gold costs; PoC: 1 cell, 1 free stall.
/// </summary>
public class StallPlacer : MonoBehaviour
{
    public string stallName = "Tteokbokki";
    public Color stallColor = new Color(0.95f, 0.45f, 0.15f);

    public bool IsOccupied { get; private set; }

    void OnMouseDown()
    {
        if (IsOccupied)
        {
            Debug.Log("[Yajang PoC] Cell already occupied.");
            return;
        }

        IsOccupied = true;
        PocVisuals.DrawStall(transform.position, stallColor, stallName);
        Debug.Log("[Yajang PoC] Placed " + stallName + " stall at " + transform.position + ".");
    }
}
