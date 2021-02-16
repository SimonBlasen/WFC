using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlapWFC
{
    public class Slot
    {
        private List<int> domains = new List<int>();
        private PatternManager patternManager;

        public Slot(PatternManager patternManager)
        {
            this.patternManager = patternManager;
            for (int i = 0; i < patternManager.PatternsAmount; i++)
            {
                domains.Add(i);
            }

            calcEntropy();
        }

        public int[] Domain
        {
            get
            {
                return domains.ToArray();
            }
        }

        public int GetDomainAt(int i)
        {
            if (i >= 0 && i < DomainSize)
            {
                return domains[i];
            }

            return -1;
        }

        public int DomainSize
        {
            get
            {
                return domains.Count;
            }
        }

        public void RemoveDomain(int index)
        {
            domains.Remove(index);

            calcEntropy();
        }


        public void Collapse()
        {
            float totalWeightSum = 0f;
            for (int i = 0; i < domains.Count; i++)
            {
                totalWeightSum += patternManager.GetPattern(domains[i]).RelativeFrequency;
            }

            float randValue = Random.Range(0f, totalWeightSum);

            int collapseIndex = -1;
            totalWeightSum = 0f;
            for (int i = 0; i < domains.Count; i++)
            {
                totalWeightSum += patternManager.GetPattern(domains[i]).RelativeFrequency;

                if (randValue <= totalWeightSum)
                {
                    collapseIndex = i;
                    break;
                }
            }

            if (collapseIndex == -1)
            {
                collapseIndex = domains.Count - 1;
            }

            int remainingIndex = domains[collapseIndex];

            domains = new List<int>();
            domains.Add(remainingIndex);

            calcEntropy();
        }

        public float Entropy
        {
            get; protected set;
        }

        private void calcEntropy()
        {
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
    }
}