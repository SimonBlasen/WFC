using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlapWFC
{
    public enum WrapStrategy
    {
        NO_PADDING, REPEAT, CLAMP, 
    }

    public class PatternManager
    {
        private int patternSize = 0;
        private List<Pattern> patterns = new List<Pattern>();

        public PatternManager(int patternSize)
        {
            this.patternSize = patternSize;
        }

        public static PatternManager CreateFromInput(int[,] values, int patternSize, WrapStrategy wrapStrategy, bool rotate = false, bool mirror = false)
        {
            PatternManager manager = new PatternManager(patternSize);

            int globalIndex = 0;

            int xStart = 0;
            int yStart = 0;
            int xMax = values.GetLength(0) - (patternSize - 1);
            int yMax = values.GetLength(1) - (patternSize - 1);

            if (wrapStrategy == WrapStrategy.CLAMP || wrapStrategy == WrapStrategy.REPEAT)
            {
                xStart = -(patternSize - 1);
                yStart = -(patternSize - 1);
                xMax = values.GetLength(0);
                yMax = values.GetLength(1);
            }


            for (int x = xStart; x < xMax; x++)
            {
                for (int y = yStart; y < yMax; y++)
                {
                    for (int rotIndex = 0; rotIndex < (rotate ? 4 : 1); rotIndex++)
                    {
                        for (int mirrorIndex = 0; mirrorIndex < (mirror ? 3 : 1); mirrorIndex++)
                        {
                            Pattern patternHere = new Pattern(globalIndex, patternSize);

                            for (int patternX = 0; patternX < patternSize; patternX++)
                            {
                                for (int patternY = 0; patternY < patternSize; patternY++)
                                {
                                    patternHere[patternX, patternY] = wrapAccess(x + patternX, y + patternY, values, wrapStrategy);
                                }
                            }



                            // Rotate and mirror

                            patternHere = patternHere.Rotate(rotIndex, globalIndex);
                            if (mirrorIndex == 1)
                            {
                                patternHere = patternHere.Mirror(false, globalIndex);
                            }
                            else if (mirrorIndex == 2)
                            {
                                patternHere = patternHere.Mirror(true, globalIndex);
                            }



                            // Check if this pattern already exists

                            bool exists = false;
                            for (int i = 0; i < manager.patterns.Count; i++)
                            {
                                if (manager.patterns[i].Match(patternHere))
                                {
                                    exists = true;
                                    manager.patterns[i].Frequency++;
                                    break;
                                }
                            }

                            if (!exists)
                            {
                                manager.patterns.Add(patternHere);
                                globalIndex++;
                            }
                        }

                    }

                }
            }



            // Generate neighbours

            for (int i = 0; i < manager.patterns.Count; i++)
            {
                for (int j = 0; j < manager.patterns.Count; j++)
                {
                    for (int oX = -(patternSize - 1); oX < patternSize; oX++)
                    {
                        for (int oY = -(patternSize - 1); oY < patternSize; oY++)
                        {
                            Vector2Int offset = new Vector2Int(oX, oY);

                            if (manager.patterns[i].Match(manager.patterns[j], offset))
                            {
                                manager.patterns[i].AddNeighbour(manager.patterns[j].Index, offset);
                            }
                        }
                    }
                }
            }

            return manager;
        }


        private static int wrapAccess(int x, int y, int[,] array, WrapStrategy wrapStrategy)
        {
            if (wrapStrategy == WrapStrategy.NO_PADDING)
            {
                if (x >= 0 && y >= 0 && x < array.GetLength(0) && y < array.GetLength(1))
                {
                    return array[x, y];
                }
                else
                {
                    return -1;
                }
            }
            else if (wrapStrategy == WrapStrategy.CLAMP)
            {
                if (x < 0)
                {
                    x = 0;
                }
                if (y < 0)
                {
                    y = 0;
                }
                if (x >= array.GetLength(0))
                {
                    x = array.GetLength(0) - 1;
                }
                if (y >= array.GetLength(1))
                {
                    y = array.GetLength(1) - 1;
                }

                return array[x, y];
            }
            else if (wrapStrategy == WrapStrategy.REPEAT)
            {
                while (x < 0)
                {
                    x += array.GetLength(0);
                }
                while (y < 0)
                {
                    y += array.GetLength(1);
                }
                while (x >= array.GetLength(0))
                {
                    x -= array.GetLength(0);
                }
                while (y >= array.GetLength(1))
                {
                    y -= array.GetLength(1);
                }

                return array[x, y];
            }

            return -1;
        }
    }

}