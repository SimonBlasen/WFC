using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public class Pattern
    {
        private int patternID = -1;

        private ulong[] bitNeighboursZ_ = new ulong[0];
        private ulong[] bitNeighboursZ = new ulong[0];


        public Pattern(int patternID)
        {
            this.patternID = patternID;
        }

        public int PatternID
        {
            get
            {
                return patternID;
            }
        }
    }

}