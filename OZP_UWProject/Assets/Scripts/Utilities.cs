using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Utilities
{
    public static Vector3 AlignToGrid(this Tilemap tilemap, Vector3 vec)
    {
        return tilemap.CellToWorld(tilemap.WorldToCell(vec));
    }
}
