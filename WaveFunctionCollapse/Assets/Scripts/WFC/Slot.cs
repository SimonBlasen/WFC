using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    //private SlotWaves possibilities;

    private List<int> possibs = new List<int>();

    public Slot(int maxElements)
    {
        //possibilities = new SlotWaves(maxElements);

        for (int i = 0; i < maxElements; i++)
        {
            possibs.Add(i);
        }
    }

    public int Entropy
    {
        get
        {
            return possibs.Count;
            //return possibilities.Entropy;
        }
    }

    public List<int> Possibles
    {
        get
        {
            return possibs;
        }
        set
        {
            possibs = value;
        }
    }
}
