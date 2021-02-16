using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGrid2D : SlotGrid
{
    protected Slot[,] slots;

    public SlotGrid2D(int width, int height, int maxElements)
    {
        Texture = new Texture2D(width, height);
        Texture.filterMode = FilterMode.Point;

        slots = new Slot[width, height];
        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                slots[x, y] = new Slot(maxElements);
            }
        }
    }

    private Vector2Int collPos = Vector2Int.zero;


    public override void CollapseSlotMinEntropy()
    {
        int minEntropy = int.MaxValue;

        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                if (slots[x, y].Entropy < minEntropy && slots[x, y].Entropy > 1)
                {
                    minEntropy = slots[x, y].Entropy;
                }
            }
        }

        List<Vector2Int> possColls = new List<Vector2Int>();


        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                if (slots[x, y].Entropy == minEntropy)
                {
                    possColls.Add(new Vector2Int(x, y));
                }
            }
        }

        int randIndex = Random.Range(0, possColls.Count);

        slots[possColls[randIndex].x, possColls[randIndex].y].Possibles.RemoveAt(Random.Range(0, slots[possColls[randIndex].x, possColls[randIndex].y].Possibles.Count));

        refreshPixelColor(possColls[randIndex]);
    }

    public override bool CollapseSlot(OverlapKernel[] kernels)
    {
        int changes = 0;

        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                refreshPixelColor(new Vector2Int(x, y));
            }
        }



        Texture.Apply();

        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                Slot slot = slots[x, y];

                if (slot.Possibles.Count == 1 && slot.Possibles[0] == 2)
                {
                    int sdfdsfe = 2;
                }
                if (slot.Possibles.Count == 1)
                {
                    int sdfdsfe = 2;
                }

                List<int>[,] remainingPosibilities = new List<int>[kernels[0].states.GetLength(0), kernels[0].states.GetLength(1)];

                for (int kX = 0; kX < remainingPosibilities.GetLength(0); kX++)
                {
                    for (int kY = 0; kY < remainingPosibilities.GetLength(1); kY++)
                    {
                        remainingPosibilities[kX, kY] = new List<int>();
                    }
                }

                bool doesNotMatchKernel = true;

                for (int i = 0; i < kernels.Length; i++)
                {
                    if (kernels[i].Match(slots, x, y))
                    {
                        doesNotMatchKernel = false;
                        for (int kX = 0; kX < remainingPosibilities.GetLength(0); kX++)
                        {
                            for (int kY = 0; kY < remainingPosibilities.GetLength(1); kY++)
                            {
                                if (remainingPosibilities[kX, kY].Contains(kernels[i].states[kX, kY]) == false)
                                {
                                    remainingPosibilities[kX, kY].Add(kernels[i].states[kX, kY]);
                                }
                            }
                        }
                    }
                }

                if (doesNotMatchKernel)
                {
                    Debug.Log("No kernel matched");
                    break;
                }


                for (int kX = 0; kX < remainingPosibilities.GetLength(0); kX++)
                {
                    for (int kY = 0; kY < remainingPosibilities.GetLength(1); kY++)
                    {
                        int gX = (x + kX) - (remainingPosibilities.GetLength(0) / 2);
                        int gY = (y + kY) - (remainingPosibilities.GetLength(1) / 2);

                        if (gX >= 0 && gY >= 0 && gX < slots.GetLength(0) && gY < slots.GetLength(1))
                        {
                            List<int> posibilitiesIntersect = new List<int>();
                            for (int i = 0; i < remainingPosibilities[kX, kY].Count; i++)
                            {
                                if (slots[gX, gY].Possibles.Contains(remainingPosibilities[kX, kY][i])
                                    && posibilitiesIntersect.Contains(remainingPosibilities[kX, kY][i]) == false)
                                {
                                    posibilitiesIntersect.Add(remainingPosibilities[kX, kY][i]);
                                }
                            }

                            if (slots[gX, gY].Possibles.Count > posibilitiesIntersect.Count)
                            {
                                changes++;
                            }

                            slots[gX, gY].Possibles = posibilitiesIntersect;
                        }
                    }
                }
            }
        }
        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                refreshPixelColor(new Vector2Int(x, y));
            }
        }



        Texture.Apply();

        return changes == 0;



        Vector2Int startPos = new Vector2Int(collPos.x, collPos.y);

        while (changes < 1)
        {
            for (int i = 0; i < slots[collPos.x, collPos.y].Possibles.Count; i++)
            {
                bool canStay = false;

                for (int j = 0; j < kernels.Length; j++)
                {
                    if (kernels[j].KernelMid == slots[collPos.x, collPos.y].Possibles[i])
                    {
                        if (kernels[j].Match(slots, collPos.x, collPos.y))
                        {
                            canStay = true;
                            break;
                        }
                    }
                }

                if (canStay == false)
                {
                    slots[collPos.x, collPos.y].Possibles.RemoveAt(i);
                    i--;
                    changes++;

                    refreshPixelColor(collPos);
                }
            }

            collPos.x++;
            if (collPos.x >= slots.GetLength(0))
            {
                collPos.x = 0;
                collPos.y++;

                if (collPos.y >= slots.GetLength(1))
                {
                    collPos.y = 0;
                }
            }

            if (collPos.x == startPos.x && collPos.y == startPos.y)
            {
                break;
            }
        }

        Texture.Apply();

        return collPos.x == startPos.x && collPos.y == startPos.y;
    }


    private void refreshPixelColor(Vector2Int pos)
    {

        Color mixedColor = new Color(0, 0, 0, 0);

        if (slots[pos.x, pos.y].Possibles.Count > 0)
        {
            for (int c = 0; c < slots[pos.x, pos.y].Possibles.Count; c++)
            {
                mixedColor += GlobalColors.Colors[slots[pos.x, pos.y].Possibles[c]];
            }

            mixedColor /= slots[pos.x, pos.y].Possibles.Count;
        }
        else
        {
            mixedColor = Color.black;
        }

        Texture.SetPixel(pos.x, pos.y, mixedColor);
    }
}
