using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public enum WrapStrategy
    {
        NO_PADDING, REPEAT, CLAMP,
    }

    [ExecuteInEditMode]
    public class InputReader : MonoBehaviour
    {
        [SerializeField]
        private bool loadResources = false;
        [SerializeField]
        private bool computePrototypes = false;
        [SerializeField]
        private bool calculateInput = false;
        [SerializeField]
        private int tempVisualizePrototypeIndex = 2;
        [SerializeField]
        private int tempVisualizePrototypeIndex0 = 2;

        [Space]

        [SerializeField]
        private int patternSize = 2;
        [SerializeField]
        private Vector3 inputGridSize = new Vector3(3f, 1.1f, 3f);
        [SerializeField]
        private bool mirror = false;
        [SerializeField]
        private bool rotate = false;
        [SerializeField]
        private WrapStrategy wrapStrategy = WrapStrategy.NO_PADDING;
        [SerializeField]
        private Vector3Int verticesPerDimension = new Vector3Int(4, 4, 4);

        [Space]

        [Header("References")]
        [SerializeField]
        private string resourcesFolderPath = "";

        private int[,,] inputValues = null;

        private PatternManager patternManager;

        private int _spawnRadius = 1;
        private Vector2Int _spawnRadiusCoord = new Vector2Int(0, 0);


        public Vector3 InputGridSize
        {
            get
            {
                return inputGridSize;
            }
        }

        void Start()
        {

        }

        void Update()
        {
            if (calculateInput)
            {
                calculateInput = false;

                calcInput();
            }

            if (computePrototypes)
            {
                computePrototypes = false;

                makePrototypes();
            }

            if (loadResources)
            {
                loadResources = false;

                loadModelsFromResources();
            }
        }

        private void makePrototypes()
        {
            int tempVisCounter = 0;

            Transform prototypesParent = null;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "Prototypes Parent")
                {
                    prototypesParent = transform.GetChild(i);
                    break;
                }
            }

            if (prototypesParent == null)
            {
                Debug.LogError("Prototypes Parent not found", gameObject);
                return;
            }


            InputElement[] inputElements = transform.GetComponentsInChildren<InputElement>();

            MeshAnalyzer meshAnalyzer = new MeshAnalyzer(inputGridSize);
            for (int i = 0; i < inputElements.Length; i++)
            {
                AnalyzedMesh analyzedMesh = meshAnalyzer.AnalyzeMesh(inputElements[i].gameObject, tempVisCounter == tempVisualizePrototypeIndex || tempVisCounter == tempVisualizePrototypeIndex0);
                tempVisCounter++;
                inputElements[i].AnalyzedMesh = analyzedMesh;

                List<AnalyzedMesh> meshesToCheckAgainst = new List<AnalyzedMesh>();
                meshesToCheckAgainst.Add(analyzedMesh);


                // TODO Make prototypes

                for (int j = 1; j < 8; j++)
                {
                    bool isMirrored = j >= 4;
                    int rotationIndex = j % 4;

                    //AnalyzedMesh rotMirMesh = AnalyzedMesh.RotatedMirroredMesh(analyzedMesh, rotationIndex, isMirrored);


                    InputElement instInputElement = createInputElement(inputElements[i].ResourcesPrefab);

                    instInputElement.transform.GetChild(0).localRotation = Quaternion.Euler(0f, rotationIndex * 90f, 0f); ;
                    instInputElement.transform.GetChild(0).localScale = new Vector3(isMirrored ? -1f : 1f, 1f, 1f);


                    instInputElement.AnalyzedMesh = meshAnalyzer.AnalyzeMesh(instInputElement.gameObject, tempVisCounter == tempVisualizePrototypeIndex || tempVisCounter == tempVisualizePrototypeIndex0);
                    tempVisCounter++;

                    if (isAnalyzedMeshEqualtoList(instInputElement.AnalyzedMesh, meshesToCheckAgainst))
                    {
                        DestroyImmediate(instInputElement.gameObject);
                    }
                    else
                    {
                        meshesToCheckAgainst.Add(instInputElement.AnalyzedMesh);
                    }
                }
            }

        }


        private bool isAnalyzedMeshEqualtoList(AnalyzedMesh checkMesh, List<AnalyzedMesh> toCheckMeshes)
        {
            for (int i = 0; i < toCheckMeshes.Count; i++)
            {
                if (toCheckMeshes[i].IsEqualTo(checkMesh))
                {
                    return true;
                }
            }
            return false;
        }


        private void loadModelsFromResources()
        {

            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = transform.GetChild(i);
            }

            for (int i = 0; i < children.Length; i++)
            {
                DestroyImmediate(children[i].gameObject);
            }

            Debug.Log("Destroyed all children");

            // Make prototypes parent
            GameObject prototypesParent = new GameObject("Prototypes Parent");
            prototypesParent.transform.parent = transform;




            GameObject[] allObjects = Resources.LoadAll<GameObject>(resourcesFolderPath);

            _spawnRadius = 1;
            _spawnRadiusCoord = new Vector2Int(0, 0);
            for (int i = 0; i < allObjects.Length; i++)
            {
                createInputElement(allObjects[i]);
            }
        }

        private InputElement createInputElement(GameObject resourcesPrefab)
        {
            GameObject instInputElement = new GameObject("Input Element [" + resourcesPrefab.name + "]");
            instInputElement.transform.parent = transform;
            InputElement inputElement = instInputElement.AddComponent<InputElement>();

            inputElement.ResourcesPrefab = resourcesPrefab;

            GameObject instModel = Instantiate(resourcesPrefab, instInputElement.transform);
            instModel.transform.localPosition = Vector3.zero;
            instModel.transform.localRotation = Quaternion.identity;
            instModel.transform.localScale = new Vector3(1f, 1f, 1f);

            instInputElement.transform.localPosition = Vector3.Scale(inputGridSize, new Vector3(_spawnRadiusCoord.x, 0f, _spawnRadiusCoord.y));

            if (_spawnRadiusCoord.x + 1 < _spawnRadius)
            {
                _spawnRadiusCoord.x++;
                _spawnRadiusCoord.y = _spawnRadius - 1;
            }

            // Jump to Y
            else
            {
                _spawnRadiusCoord.y--;
                _spawnRadiusCoord.x = _spawnRadius - 1;
            }

            // Is finished
            if (_spawnRadiusCoord.y < 0)
            {
                _spawnRadius++;
                _spawnRadiusCoord = new Vector2Int(0, _spawnRadius - 1);
            }

            return inputElement;
        }

        private void calcInput()
        {
            InputElement[] inputElements = transform.GetComponentsInChildren<InputElement>();

            patternManager = PatternManager.CreateFromInput(inputElements, inputGridSize);
        }
    }

}