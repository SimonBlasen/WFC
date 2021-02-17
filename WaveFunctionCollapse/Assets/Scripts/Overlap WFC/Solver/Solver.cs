using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlapWFC
{
    public class Solver
    {
        private const float entropyWiggle = 0.01f;

        private PatternManager patternManager;
        private Slot[,] slots;

        public Solver(Vector2Int outputSize, PatternManager patternManager)
        {
            slots = new Slot[outputSize.x, outputSize.y];
            this.patternManager = patternManager;

            for (int x = 0; x < slots.GetLength(0); x++)
            {
                for (int y = 0; y < slots.GetLength(1); y++)
                {
                    slots[x, y] = new Slot(patternManager);
                }
            }
        }

        public Slot[,] Slots
        {
            get
            {
                return slots;
            }
        }

        public bool Iterate()
        {
            List<Vector2Int> stack = new List<Vector2Int>();
            collapseMaxEntropy(stack);

            bool allCollapsed = false;
            while (!allCollapsed)
            {
                allCollapsed = iterateConstraints(stack);
            }

            return collapsedCompletely();
        }

        private bool iterateConstraints(List<Vector2Int> stack)
        {
            bool didSthChange = false;

            while (stack.Count > 0)
            {
                int x = stack[0].x;
                int y = stack[0].y;

                stack.RemoveAt(0);

                //for (int offsetX = -(patternManager.PatternSize - 1); offsetX < patternManager.PatternSize; offsetX++)
                for (int offsetX = -1; offsetX < 2; offsetX++)
                {
                    //for (int offsetY = -(patternManager.PatternSize - 1); offsetY < patternManager.PatternSize; offsetY++)
                    for (int offsetY = -1; offsetY < 2; offsetY++)
                    {
                        //List<int> possiblePatterns = new List<int>();
                        //List<int> domainsAtOffset = new List<int>();

                        int ofPosX = x + offsetX;
                        int ofPosY = y + offsetY;


                        if (ofPosX >= 0 && ofPosX < slots.GetLength(0)
                            && ofPosY >= 0 && ofPosY < slots.GetLength(1))
                        {
                            ulong[] domainsAtOffset = slots[ofPosX, ofPosY].Domain;
                            ulong[] domainsAtOffsetOld = new ulong[domainsAtOffset.Length];
                            ulong[] possibleNeighbours = slots[x, y].PossibleNeighboursAt(new Vector2Int(offsetX, offsetY));// new ulong[domainsAtOffset.Length];
                            for (int arr = 0; arr < possibleNeighbours.Length; arr++)
                            {
                                //possibleNeighbours[arr] = 0x0UL;
                                domainsAtOffsetOld[arr] = domainsAtOffset[arr];
                            }
                            //domainsAtOffset.AddRange(slots[ofPosX, ofPosY].Domain);

                            /*for (int i = 0; i < slots[x, y].DomainSize; i++)
                            {
                                Pattern patternHere = patternManager.GetPattern(slots[x, y].GetDomainAt(i));

                                ulong[] neighbours = patternHere.NeighboursAt(new Vector2Int(offsetX, offsetY));

                                for (int arr = 0; arr < neighbours.Length; arr++)
                                {
                                    possibleNeighbours[arr] |= neighbours[arr];
                                    //domainsAtOffset[arr] &= neighbours[arr];
                                }
                            }*/

                            bool changed = false;
                            for (int arr = 0; arr < possibleNeighbours.Length; arr++)
                            {
                                //possibleNeighbours[arr] |= neighbours[arr];
                                domainsAtOffset[arr] &= possibleNeighbours[arr];
                                if (domainsAtOffset[arr] != domainsAtOffsetOld[arr])
                                {
                                    changed = true;
                                }
                            }


                            if (changed)
                            {
                                int prevDomSize = slots[ofPosX, ofPosY].DomainSize;
                                slots[ofPosX, ofPosY].RecalculateDomainSize();

                                if (prevDomSize > slots[ofPosX, ofPosY].DomainSize)
                                {
                                    stack.Add(new Vector2Int(ofPosX, ofPosY));
                                }
                            }

                            /*
                            // Reduce domains
                            for (int i = 0; i < possiblePatterns.Count; i++)
                            {
                                domainsAtOffset.Remove(possiblePatterns[i]);
                            }

                            if (domainsAtOffset.Count > 0)
                            {
                                stack.Add(new Vector2Int(ofPosX, ofPosY));
                            }

                            for (int i = 0; i < domainsAtOffset.Count; i++)
                            {
                                didSthChange = true;
                                slots[ofPosX, ofPosY].RemoveDomain(domainsAtOffset[i]);
                            }*/
                        }
                    }
                }
            }

            /*
            for (int x = 0; x < slots.GetLength(0); x++)
            {
                for (int y = 0; y < slots.GetLength(1); y++)
                {
                    
                }
            }
            */

            return !didSthChange;
        }

        private void collapseMaxEntropy(List<Vector2Int> stack)
        {
            float max = float.MaxValue;
            Slot collapseSlot = null;
            Vector2Int collapsedPos = Vector2Int.zero;
            for (int x = 0; x < slots.GetLength(0); x++)
            {
                for (int y = 0; y < slots.GetLength(1); y++)
                {
                    float wiggledEntropy = slots[x, y].Entropy + Random.Range(0f, entropyWiggle);
                    if (wiggledEntropy < max && slots[x, y].DomainSize > 1)
                    {
                        max = wiggledEntropy;
                        collapseSlot = slots[x, y];
                        collapsedPos = new Vector2Int(x, y);
                    }
                }
            }

            if (collapseSlot == null)
            {
                Debug.LogError("No slot to collapse with max entropy found");
            }
            else
            {
                Debug.Log("Took entropy: " + max.ToString("n4"));
                stack.Add(collapsedPos);
                collapseSlot.Collapse();
            }
        }

        private bool collapsedCompletely()
        {
            for (int x = 0; x < slots.GetLength(0); x++)
            {
                for (int y = 0; y < slots.GetLength(1); y++)
                {
                    if (slots[x, y].DomainSize > 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

}