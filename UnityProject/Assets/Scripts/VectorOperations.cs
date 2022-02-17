using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorOperations
{
    public static Vector2Int Vector3ToVector2Int(Vector3 position)
    {
        Vector3Int pos = Vector3Int.RoundToInt(position);
        return new Vector2Int(pos.x, pos.z);
    }
}
