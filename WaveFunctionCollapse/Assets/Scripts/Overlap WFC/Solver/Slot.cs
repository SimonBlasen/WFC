using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlapWFC
{
    public class Slot
    {
        //private List<int> domains = new List<int>();
        private ulong[] bitDomain = new ulong[0];

        private ulong[] bitNeighbours_1_1 = new ulong[0];
        private ulong[] bitNeighbours_0_0 = new ulong[0];
        private ulong[] bitNeighbours_0_1 = new ulong[0];
        private ulong[] bitNeighbours_0_2 = new ulong[0];
        private ulong[] bitNeighbours_1_2 = new ulong[0];
        private ulong[] bitNeighbours_2_2 = new ulong[0];
        private ulong[] bitNeighbours_2_1 = new ulong[0];
        private ulong[] bitNeighbours_2_0 = new ulong[0];
        private ulong[] bitNeighbours_1_0 = new ulong[0];

        private PatternManager patternManager;

        public Slot(PatternManager patternManager)
        {
            this.patternManager = patternManager;

            bitDomain = new ulong[((patternManager.PatternsAmount - 1) / 64) + 1];

            DomainSize = patternManager.PatternsAmount;

            for (int i = 0; i < bitDomain.Length; i++)
            {
                bitDomain[i] = 0xFFFFFFFFFFFFFFFF;
            }

            for (int i = patternManager.PatternsAmount; i < bitDomain.Length * 64; i++)
            {
                int arr = i / 64;
                int bit = i - arr * 64;

                bitDomain[arr] &= ~(0x1UL << bit);
            }

            bitNeighbours_0_0 = new ulong[bitDomain.Length];
            bitNeighbours_0_1 = new ulong[bitDomain.Length];
            bitNeighbours_0_2 = new ulong[bitDomain.Length];
            bitNeighbours_1_0 = new ulong[bitDomain.Length];
            bitNeighbours_1_1 = new ulong[bitDomain.Length];
            bitNeighbours_1_2 = new ulong[bitDomain.Length];
            bitNeighbours_2_0 = new ulong[bitDomain.Length];
            bitNeighbours_2_1 = new ulong[bitDomain.Length];
            bitNeighbours_2_2 = new ulong[bitDomain.Length];

            domainIndicer = new int[DomainSize];
            /*for (int i = 0; i < DomainSize; i++)
            {
                domainIndicer[i] = i;
            }*/

            /*for (int i = 0; i < bitDomain.Length; i++)
            {
                string strRep = System.Convert.ToString((long)bitDomain[i], 2);
                int sdfew = 0;
            }*/

            recalculateDomainIndicer();

            recalculatePossibleNeighbours();

            calcEntropy();
        }

        public ulong[] Domain
        {
            get
            {
                return bitDomain;
            }
        }

        /*public int[] Domain
        {
            get
            {
                return domains.ToArray();
            }
        }*/

        private int[] domainIndicer = new int[0];

        public int GetDomainAt(int i)
        {
            return domainIndicer[i];

            /*int index = 0;
            int counter = 0;

            while (counter <= i)
            {
                int arr = index / 64;
                int bit = index - arr * 64;

                if (((bitDomain[arr] >> index) & 0x1UL) == 0x1UL)
                {
                    counter++;
                }

                index++;
            }

            return index - 1;*/

            /*
            if (i >= 0 && i < DomainSize)
            {
                return 0;// return domains[i];
            }

            return -1;*/
        }

        public int DomainSize
        {
            get; protected set;
        } = 0;

        public void RecalculateDomainSize()
        {
            DomainSize = 0;

            for (int i = 0; i < bitDomain.Length; i++)
            {
                DomainSize += sparseBitcount(bitDomain[i]);
            }

            recalculateDomainIndicer();

            recalculatePossibleNeighbours();

            calcEntropy();
        }

        private static int sparseBitcount(ulong n)
        {
            int count = 0;
            while (n != 0)
            {
                count++;
                n &= (n - 1);
            }
            return count;
        }

        /*public void RemoveDomain(int index)
        {
            DomainSize--;

            int arr = index / 64;
            int bit = index - arr * 64;

            bitDomain[arr] = (0x1UL << bit);

            //domains.Remove(index);

            calcEntropy();
        }*/


        public void Collapse()
        {
            List<int> patternsLeft = new List<int>();
            for (int i = 0; i < DomainSize; i++)
            {
                patternsLeft.Add(GetDomainAt(i));
            }
            
            float totalWeightSum = 0f;
            for (int i = 0; i < patternsLeft.Count; i++)
            {
                totalWeightSum += patternManager.GetPattern(patternsLeft[i]).RelativeFrequency;
            }

            float randValue = Random.Range(0f, totalWeightSum);

            int collapseIndex = -1;
            totalWeightSum = 0f;
            for (int i = 0; i < patternsLeft.Count; i++)
            {
                totalWeightSum += patternManager.GetPattern(patternsLeft[i]).RelativeFrequency;

                if (randValue <= totalWeightSum)
                {
                    collapseIndex = i;
                    break;
                }
            }

            if (collapseIndex == -1)
            {
                collapseIndex = patternsLeft.Count - 1;
            }

            int remainingIndex = patternsLeft[collapseIndex];

            int arr = remainingIndex / 64;
            int bit = remainingIndex - arr * 64;

            for (int i = 0; i < bitDomain.Length; i++)
            {
                bitDomain[i] = 0x0UL;
            }

            bitDomain[arr] = (0x1UL << bit);

            DomainSize = 1;

            recalculateDomainIndicer();

            recalculatePossibleNeighbours();

            calcEntropy();
        }

        public float Entropy
        {
            get; protected set;
        }

        private void calcEntropy()
        {
            List<int> domains = new List<int>();
            for (int i = 0; i < DomainSize; i++)
            {
                domains.Add(GetDomainAt(i));
            }

            float totalWeightSum = 0f;
            float partialLogSum = 0f;
            for (int i = 0; i < domains.Count; i++)
            {
                totalWeightSum += patternManager.GetPattern(domains[i]).RelativeFrequency;
                partialLogSum += Mathf.Log(patternManager.GetPattern(domains[i]).RelativeFrequency, 2f);
            }

            float log2TotalWeight = Mathf.Log(totalWeightSum, 2f);

            float entropy = log2TotalWeight - (partialLogSum / totalWeightSum);

            Entropy = entropy;
        }

        public ulong[] PossibleNeighboursAt(Vector2Int offset)
        {
            if (offset.x == -1 && offset.y == -1)
                return bitNeighbours_0_0;
            else if (offset.x == -1 && offset.y == 0)
                return bitNeighbours_0_1;
            else if (offset.x == -1 && offset.y == 1)
                return bitNeighbours_0_2;
            else if (offset.x == 0 && offset.y == -1)
                return bitNeighbours_1_0;
            else if (offset.x == 0 && offset.y == 0)
                return bitNeighbours_1_1;
            else if (offset.x == 0 && offset.y == 1)
                return bitNeighbours_1_2;
            else if (offset.x == 1 && offset.y == -1)
                return bitNeighbours_2_0;
            else if (offset.x == 1 && offset.y == 0)
                return bitNeighbours_2_1;
            else if (offset.x == 1 && offset.y == 1)
                return bitNeighbours_2_2;
            else
                return null;
        }

        private void recalculateDomainIndicer()
        {
            int counter = 0;

            for (int index = 0; index < bitDomain.Length * 64; index++)
            {
                int arr = index / 64;
                int bit = index - arr * 64;

                if (((bitDomain[arr] >> bit) & 0x1UL) == 0x1UL)
                {
                    domainIndicer[counter] = index;
                    counter++;
                }
            }
        }

        private void recalculatePossibleNeighbours()
        {
            for (int offsetX = -1; offsetX < 2; offsetX++)
            {
                for (int offsetY = -1; offsetY < 2; offsetY++)
                {
                    ulong[] possibleNeighbours = new ulong[bitDomain.Length];
                    for (int arr = 0; arr < possibleNeighbours.Length; arr++)
                    {
                        possibleNeighbours[arr] = 0x0UL;
                    }

                    for (int i = 0; i < DomainSize; i++)
                    {
                        Pattern patternHere = patternManager.GetPattern(GetDomainAt(i));
                        ulong[] neighbours = patternHere.NeighboursAt(new Vector2Int(offsetX, offsetY));

                        for (int arr = 0; arr < neighbours.Length; arr++)
                        {
                            possibleNeighbours[arr] |= neighbours[arr];
                        }
                    }

                    for (int arr = 0; arr < possibleNeighbours.Length; arr++)
                    {
                        if (offsetX == -1 && offsetY == -1)
                            bitNeighbours_0_0[arr] = possibleNeighbours[arr];
                        else if (offsetX == -1 && offsetY == 0)
                            bitNeighbours_0_1[arr] = possibleNeighbours[arr];
                        else if (offsetX == -1 && offsetY == 1)
                            bitNeighbours_0_2[arr] = possibleNeighbours[arr];
                        else if (offsetX == 0 && offsetY == -1)
                            bitNeighbours_1_0[arr] = possibleNeighbours[arr];
                        else if (offsetX == 0 && offsetY == 0)
                            bitNeighbours_1_1[arr] = possibleNeighbours[arr];
                        else if (offsetX == 0 && offsetY == 1)
                            bitNeighbours_1_2[arr] = possibleNeighbours[arr];
                        else if (offsetX == 1 && offsetY == -1)
                            bitNeighbours_2_0[arr] = possibleNeighbours[arr];
                        else if (offsetX == 1 && offsetY == 0)
                            bitNeighbours_2_1[arr] = possibleNeighbours[arr];
                        else if (offsetX == 1 && offsetY == 1)
                            bitNeighbours_2_2[arr] = possibleNeighbours[arr];
                    }
                }
            }
        }
    }
}