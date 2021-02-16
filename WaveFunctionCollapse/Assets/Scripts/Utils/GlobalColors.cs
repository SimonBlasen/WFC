using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalColors : MonoBehaviour
{
    public Color[] colors;

    private static GlobalColors inst = null;

    public static Color[] Colors
    {
        get
        {
            if (inst == null)
            {
                inst = GameObject.FindObjectOfType<GlobalColors>();
            }

            return inst.colors;
        }
    }
}
