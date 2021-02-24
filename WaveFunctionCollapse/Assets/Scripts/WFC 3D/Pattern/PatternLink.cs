using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public class PatternLink
    {
        private Pattern pattern;
        private GameObject prefab;

        private int rotationIndex = 0;
        private bool mirrored = false;

        public PatternLink(Pattern pattern, GameObject prefab)
        {
            this.pattern = pattern;
            this.prefab = prefab;
        }
    }
}
