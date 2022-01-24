using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static bool AreNormalsEqual(Vector3Int normalA, Vector3Int normalB)
    {
        float angle = Vector3.Angle(normalA, normalB);

        return angle < 90f;
    }
}
