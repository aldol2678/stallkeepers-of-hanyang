using UnityEngine;

/// <summary>
/// Yajang Defense PoC — procedural 2D visuals (coloured quads, zero binary assets).
/// The PoC deliberately uses no textures, sprites or models so the project is
/// text files only and nothing can fail on a missing asset import.
/// Sorting: path 0, cell 1, stall 2, enemy 3.
/// </summary>
public static class PocVisuals
{
    private static Material _sharedMaterial;

    private static Material SharedMaterial()
    {
        if (_sharedMaterial == null)
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            _sharedMaterial = new Material(shader);
        }
        return _sharedMaterial;
    }

    /// <summary>Creates a flat coloured quad and removes its 3D MeshCollider.</summary>
    public static GameObject DrawQuad(string name, Vector3 position, Vector3 size, Color color, int sortingOrder)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.name = name;
        go.transform.position = position;
        go.transform.localScale = size;

        MeshRenderer renderer = go.GetComponent<MeshRenderer>();
        renderer.material = new Material(SharedMaterial()) { color = color };
        renderer.sortingOrder = sortingOrder;

        Collider collider = go.GetComponent<Collider>();
        if (collider != null) Object.Destroy(collider);

        return go;
    }

    /// <summary>Draws the enemy path as one quad per waypoint segment.</summary>
    public static void DrawPath(Transform[] waypoints)
    {
        Color pathColor = new Color(0.45f, 0.38f, 0.28f);
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector3 a = waypoints[i].position;
            Vector3 b = waypoints[i + 1].position;
            Vector3 mid = (a + b) * 0.5f;
            float length = Vector3.Distance(a, b);
            float angle = Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;

            GameObject segment = DrawQuad(
                "PathSegment_" + i, mid, new Vector3(length, 0.9f, 1f), pathColor, 0);
            segment.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    /// <summary>Draws the clickable placement cell (green, semi-transparent).</summary>
    public static void DrawCell(Vector3 position)
    {
        DrawQuad("CellHighlight", position,
            new Vector3(1.6f, 1.6f, 1f), new Color(0.25f, 0.8f, 0.35f, 0.55f), 1);
    }

    /// <summary>Draws the Tteokbokki stall: orange body + darker roof.</summary>
    public static void DrawStall(Vector3 position, Color color, string stallName)
    {
        GameObject stall = DrawQuad("Stall_" + stallName, position,
            new Vector3(1.4f, 1.0f, 1f), color, 2);
        stall.transform.position = new Vector3(position.x, position.y - 0.2f, position.z);
        DrawQuad("StallRoof_" + stallName,
            new Vector3(position.x, position.y + 0.55f, position.z),
            new Vector3(1.6f, 0.35f, 1f), new Color(0.55f, 0.1f, 0.1f), 2);
    }

    /// <summary>Draws the enemy token (dark red quad) parented to the enemy object.</summary>
    public static void DrawEnemy(Transform enemy)
    {
        GameObject token = DrawQuad("EnemyToken", enemy.position,
            new Vector3(0.8f, 0.8f, 1f), new Color(0.6f, 0.12f, 0.18f), 3);
        token.transform.SetParent(enemy, worldPositionStays: true);
    }
}
