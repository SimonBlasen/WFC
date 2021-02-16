using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfLong
{
    private long[] dat = new long[1];
    private bool defaultValOne = false;

    public InfLong(int bitsAmount, int defaultVal)
    {
        int arrayLen = ((bitsAmount - 1) / 64) + 1;

        dat = new long[arrayLen];
        defaultVal = defaultVal > 0 ? 1 : 0;
        defaultValOne = defaultVal == 1;

        for (int i = 0; i < dat.Length; i++)
        {
            dat[i] = defaultValOne ? (0xFFFFFFFF) : (0x0);
        }
    }

    private static void extendArrays(InfLong a, InfLong b)
    {
        if (a.dat.Length == b.dat.Length)
        {
            return;
        }
        else if (a.dat.Length > b.dat.Length)
        {
            long[] newDat = new long[a.dat.Length];
            for (int i = 0; i < b.dat.Length; i++)
            {
                newDat[i] = b.dat[i];
            }
            for (int i = b.dat.Length; i < newDat.Length; i++)
            {
                newDat[i] = b.defaultValOne ? (0xFFFFFFFF) : (0x0);
            }

            b.dat = newDat;
        }
        else
        {
            long[] newDat = new long[b.dat.Length];
            for (int i = 0; i < a.dat.Length; i++)
            {
                newDat[i] = a.dat[i];
            }
            for (int i = a.dat.Length; i < newDat.Length; i++)
            {
                newDat[i] = a.defaultValOne ? (0xFFFFFFFF) : (0x0);
            }

            a.dat = newDat;
        }
    }
    /*
    public static InfLong operator &(InfLong a, InfLong b)
    {
        extendArrays(a, b);


    }*/
}
