using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public class PatternManager
    {
        MeshAnalyzer meshAnalyzer;

        private List<PatternLink> patternLinks = new List<PatternLink>();

        public PatternManager()
        {
            meshAnalyzer = new MeshAnalyzer();
        }


        public static PatternManager CreateFromInput(InputElement[] inputElements)
        {
            PatternManager pm = new PatternManager();

            for (int i = 0; i < inputElements.Length; i++)
            {

            }
        }
    }

}