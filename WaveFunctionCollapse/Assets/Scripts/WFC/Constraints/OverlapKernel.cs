using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapKernel
{
    public int[,] states;

    public int KernelMid
    {
        get
        {
            return states[states.GetLength(0) / 2, states.GetLength(1) / 2];
        }
    }

    public bool Match(Slot[,] slots, int midX, int midY)
    {
        int kernelRadius = states.GetLength(0) / 2;
        for (int x = 0; x < states.GetLength(0); x++)
        {
            for (int y = 0; y < states.GetLength(1); y++)
            {
                int slotPosX = (midX + x) - kernelRadius;
                int slotPosY = (midY + y) - kernelRadius;

                if (slotPosX >= 0 && slotPosY >= 0 && slotPosX < slots.GetLength(0) && slotPosY < slots.GetLength(1))
                {
                    if (slots[slotPosX, slotPosY].Possibles.Contains(states[x, y]) == false)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
