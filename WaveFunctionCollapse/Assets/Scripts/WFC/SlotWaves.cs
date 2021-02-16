using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotWaves
{
    private ulong[] longs = new ulong[0];

    private int elementsAmount = 0;

    public SlotWaves(int elements)
    {
        elementsAmount = elements;
        int arrayLen = ((elements - 1) / 64) + 1;

        longs = new ulong[arrayLen];
        for (int i = 0; i < longs.Length; i++)
        {
            longs[i] = 0xFFFFFFFF;
        }

        Entropy = elements;
    }

    public int Entropy
    {
        get; protected set;
    }
}
