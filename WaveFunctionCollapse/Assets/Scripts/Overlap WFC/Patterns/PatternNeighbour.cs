using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlapWFC
{
    public class PatternNeighbour
    {
        private int neighbourIndex = -1;
        private Vector2Int offset = Vector2Int.zero;

        public PatternNeighbour(int patternIndex, Vector2Int offset)
        {
            neighbourIndex = patternIndex;
            this.offset = offset;
        }

        public Vector2Int Offset
        {
            get
            {
                return offset;
            }
        }

        public int NeighbourIndex
        {
            get
            {
                return neighbourIndex;
            }
        }
    }

}