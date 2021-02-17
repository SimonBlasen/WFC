using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlapWFC
{
    public class Pattern
    {
        private int index = -1;
        private int size = -1;
        private int frequency = 1;
        private float relativeFrequency = 1f;

        private int[,] pattern = null;

        private ulong[] bitNeighbours_1_1 = new ulong[0];
        private ulong[] bitNeighbours_0_0 = new ulong[0];
        private ulong[] bitNeighbours_0_1 = new ulong[0];
        private ulong[] bitNeighbours_0_2 = new ulong[0];
        private ulong[] bitNeighbours_1_2 = new ulong[0];
        private ulong[] bitNeighbours_2_2 = new ulong[0];
        private ulong[] bitNeighbours_2_1 = new ulong[0];
        private ulong[] bitNeighbours_2_0 = new ulong[0];
        private ulong[] bitNeighbours_1_0 = new ulong[0];


        //private List<PatternNeighbour> neighbours = new List<PatternNeighbour>();

        public Pattern(int index, int size)
        {
            this.index = index;
            this.size = size;

            pattern = new int[size, size];
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public int this[int x, int y]
        {
            get
            {
                return pattern[x, y];
            }
            set
            {
                pattern[x, y] = value;
            }
        }

        public int Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }

        public float RelativeFrequency
        {
            get
            {
                return relativeFrequency;
            }
            set
            {
                relativeFrequency = value;
            }
        }

        public void SetMaxNeighboursAmount(int amount)
        {
            bitNeighbours_0_0 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_0_0.Length; i++)
            {
                bitNeighbours_0_0[i] = 0x0;
            }
            bitNeighbours_0_1 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_0_1.Length; i++)
            {
                bitNeighbours_0_1[i] = 0x0;
            }
            bitNeighbours_0_2 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_0_2.Length; i++)
            {
                bitNeighbours_0_2[i] = 0x0;
            }
            bitNeighbours_1_0 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_1_0.Length; i++)
            {
                bitNeighbours_1_0[i] = 0x0;
            }
            bitNeighbours_1_1 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_1_1.Length; i++)
            {
                bitNeighbours_1_1[i] = 0x0;
            }
            bitNeighbours_1_2 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_1_2.Length; i++)
            {
                bitNeighbours_1_2[i] = 0x0;
            }
            bitNeighbours_2_0 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_2_0.Length; i++)
            {
                bitNeighbours_2_0[i] = 0x0;
            }
            bitNeighbours_2_1 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_2_1.Length; i++)
            {
                bitNeighbours_2_1[i] = 0x0;
            }
            bitNeighbours_2_2 = new ulong[((amount - 1) / 64) + 1];
            for (int i = 0; i < bitNeighbours_2_2.Length; i++)
            {
                bitNeighbours_2_2[i] = 0x0;
            }
        }

        public int NeighboursAmount
        {
            get; protected set;
        } = 0;

        public ulong[] NeighboursAt(Vector2Int offset)
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

        public void AddNeighbour(int index, Vector2Int offset)
        {
            NeighboursAmount++;
            int arr = index / 64;
            int bit = index - arr * 64;

            if (offset.x == -1 && offset.y == -1)
                bitNeighbours_0_0[arr] |= (0x1UL << bit);
            else if (offset.x == -1 && offset.y == 0)
                bitNeighbours_0_1[arr] |= (0x1UL << bit);
            else if (offset.x == -1 && offset.y == 1)
                bitNeighbours_0_2[arr] |= (0x1UL << bit);
            else if (offset.x == 0 && offset.y == -1)
                bitNeighbours_1_0[arr] |= (0x1UL << bit);
            else if (offset.x == 0 && offset.y == 0)
                bitNeighbours_1_1[arr] |= (0x1UL << bit);
            else if (offset.x == 0 && offset.y == 1)
                bitNeighbours_1_2[arr] |= (0x1UL << bit);
            else if (offset.x == 1 && offset.y == -1)
                bitNeighbours_2_0[arr] |= (0x1UL << bit);
            else if (offset.x == 1 && offset.y == 0)
                bitNeighbours_2_1[arr] |= (0x1UL << bit);
            else if (offset.x == 1 && offset.y == 1)
                bitNeighbours_2_2[arr] |= (0x1UL << bit);
        }


        public bool Match(Pattern other)
        {
            return Match(other, Vector2Int.zero);
        }

        public bool Match(Pattern other, Vector2Int offset)
        {
            int maxSize = Mathf.Max(size, other.size);

            for (int x = -maxSize; x < maxSize * 2; x++)
            {
                for (int y = -maxSize; y < maxSize * 2; y++)
                {
                    Vector2Int posSelf = new Vector2Int(x, y);
                    Vector2Int posOther = new Vector2Int(x + offset.x, y + offset.y);

                    if (posSelf.x >= 0 && posSelf.x < size
                        && posSelf.y >= 0 && posSelf.y < size
                        && posOther.x >= 0 && posOther.x < other.size
                        && posOther.y >= 0 && posOther.y < other.size)
                    {
                        if (this[posSelf.x, posSelf.y] != other[posOther.x, posOther.y])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public Pattern Mirror(bool xAxe, int index)
        {
            Pattern mirrored = new Pattern(index, size);

            if (xAxe)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        mirrored[size - x - 1, y] = this[x, y];
                    }
                }
            }
            else
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        mirrored[x, size - y - 1] = this[x, y];
                    }
                }
            }

            return mirrored;
        }

        public Pattern Rotate(int rotateIndex, int index)
        {
            while (rotateIndex < 0)
            {
                rotateIndex += 4;
            }
            rotateIndex = rotateIndex % 4;

            Pattern mirrored = new Pattern(index, size);

            if (rotateIndex == 0)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        mirrored[x, y] = this[x, y];
                    }
                }
            }
            else if (rotateIndex == 1)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        mirrored[y, size - x - 1] = this[x, y];
                    }
                }
            }
            else if (rotateIndex == 2)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        mirrored[size - x - 1, size - y - 1] = this[x, y];
                    }
                }
            }
            else if (rotateIndex == 3)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        mirrored[size - y - 1, x] = this[x, y];
                    }
                }
            }

            return mirrored;
        }
    }
}
