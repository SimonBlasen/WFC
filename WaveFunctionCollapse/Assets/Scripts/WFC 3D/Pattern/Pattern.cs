using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public class Pattern
    {
        private int patternID = -1;
        private int rotationIndex = 0;
        private bool 

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

        public int RotationIndex
        {
            get
            {
                return rotationIndex;
            }
            set
            {
                rotationIndex = value;
            }
        }
    }

}