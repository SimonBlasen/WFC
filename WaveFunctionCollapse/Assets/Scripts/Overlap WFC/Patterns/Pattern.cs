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

        private List<PatternNeighbour> neighbours = new List<PatternNeighbour>();

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

        public int NeighboursAmount
        {
            get
            {
                return neighbours.Count;
            }
        }

        public bool HasNeighbour(int index, Vector2Int offset)
        {
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].NeighbourIndex == index && neighbours[i].Offset == offset)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddNeighbour(int index, Vector2Int offset)
        {
            PatternNeighbour neighbour = new PatternNeighbour(index, offset);

            neighbours.Add(neighbour);
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
