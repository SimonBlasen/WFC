using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public class PatternManager
    {
        MeshAnalyzer meshAnalyzer;

        private List<PatternLink> patternLinks = new List<PatternLink>();

        public PatternManager(Vector3 gridSize)
        {
            meshAnalyzer = new MeshAnalyzer(gridSize);
        }


        public static PatternManager CreateFromInput(InputElement[] inputElements, Vector3 gridSize)
        {
            PatternManager pm = new PatternManager(gridSize);

            for (int i = 0; i < inputElements.Length; i++)
            {
                //pm.meshAnalyzer.AnalyzeFace(inputElements[i].gameObject);
            }

            return pm;
        }
    }

}