using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapser
{
    private SlotGrid grid;
    private OverlapKernel[] kernels;

    public Collapser()
    {

    }


    public void Coll(int sizeX, int sizeY, int maxElements, OverlapKernel[] kernels)
    {
        this.kernels = kernels;
        grid = new SlotGrid2D(sizeX, sizeY, maxElements);
    }

    public void Tick()
    {
        bool collapsed = grid.CollapseSlot(kernels);

        if (collapsed)
        {
            //Debug.Log("Collapsed. Reducing entropy");

            //grid.CollapseSlotMinEntropy();
        }
    }

    public void CollapseReduceEntropy()
    {
        Debug.Log("Collapsed. Reducing entropy");

        grid.CollapseSlotMinEntropy();
    }

    public Texture2D Texture
    {
        get
        {
            return grid.Texture;
        }
    }
}
