using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGrid
{
    public Texture2D Texture
    {
        get; protected set;
    }

    public virtual bool CollapseSlot(OverlapKernel[] kernels)
    {
        return true;
    }

    public virtual void CollapseSlotMinEntropy()
    {

    }
}
