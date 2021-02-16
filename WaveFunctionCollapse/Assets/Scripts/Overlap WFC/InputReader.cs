using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlapWFC
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField]
        private bool calculateInput = false;

        [Space]

        [SerializeField]
        private int inputSize = 3;
        [SerializeField]
        private int patternSize = 2;
        [SerializeField]
        private WrapStrategy wrapStrategy = WrapStrategy.NO_PADDING;

        [Space]

        [Header("References")]
        [SerializeField]
        private Transform contentTransform = null;


        private int[,] values = null;
        private GameObject[,] meshes = null;

        private Dictionary<int, GameObject> dictMesh = new Dictionary<int, GameObject>();

        private PatternManager patternManager = null;

        public GameObject GetMesh(int index)
        {
            if (dictMesh.ContainsKey(index))
            {
                return dictMesh[index];
            }
            return null;
        }


        private void Update()
        {
            if (calculateInput)
            {
                calculateInput = false;

                calcInput();
            }


            Debug.DrawLine(new Vector3(-0.5f, -0.5f) + contentTransform.position, new Vector3(inputSize - 0.5f, -0.5f) + contentTransform.position);
            Debug.DrawLine(new Vector3(-0.5f, -0.5f) + contentTransform.position, new Vector3(-0.5f, inputSize - 0.5f) + contentTransform.position);
            Debug.DrawLine(new Vector3(-0.5f + inputSize, -0.5f) + contentTransform.position, new Vector3(-0.5f + inputSize, inputSize - 0.5f) + contentTransform.position);
            Debug.DrawLine(new Vector3(-0.5f, -0.5f + inputSize) + contentTransform.position, new Vector3(-0.5f + inputSize, inputSize - 0.5f) + contentTransform.position);
        }



        private void calcInput()
        {
            values = new int[inputSize, inputSize];
            meshes = new GameObject[inputSize, inputSize];

            dictMesh = new Dictionary<int, GameObject>();


            Dictionary<string, int> nameIndices = new Dictionary<string, int>();
            int globalIndex = 0;


            for (int x = 0; x < inputSize; x++)
            {
                for (int y = 0; y < inputSize; y++)
                {
                    Transform foundTrans = null;

                    for (int i = 0; i < contentTransform.childCount; i++)
                    {
                        Transform child = contentTransform.GetChild(i);

                        if (Vector3.Distance(child.localPosition, new Vector3(x, y, 0f)) <= 0.05f)
                        {
                            foundTrans = child;
                            break;
                        }
                    }

                    string key = "NONE";

                    if (foundTrans != null)
                    {
                        key = foundTrans.name.Substring(0, 4);
                    }

                    int index = -1;

                    if (nameIndices.ContainsKey(key))
                    {
                        index = nameIndices[key];
                    }
                    else
                    {
                        nameIndices.Add(key, globalIndex);
                        index = globalIndex;
                        globalIndex++;
                    }

                    values[x, y] = index;
                    if (foundTrans != null)
                    {
                        meshes[x, y] = foundTrans.gameObject;
                        if (dictMesh.ContainsKey(index) == false)
                        {
                            dictMesh.Add(index, foundTrans.gameObject);
                        }
                    }
                    else
                    {
                        meshes[x, y] = null;
                    }
                }
            }

            patternManager = PatternManager.CreateFromInput(values, patternSize, wrapStrategy, true, true);
        }


        public PatternManager PatternManager
        {
            get
            {
                return patternManager;
            }
        }
    }

}