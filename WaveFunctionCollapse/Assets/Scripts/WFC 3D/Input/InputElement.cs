using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WFC3D
{
    [ExecuteInEditMode]
    public class InputElement : MonoBehaviour
    {
        [SerializeField]
        private bool onlyRotateY = false;
        [SerializeField]
        private Vector3Int objectSize = new Vector3Int(1, 1, 1);

        [SerializeField]
        private string conn_down = "";
        [SerializeField]
        private string conn_up = "";
        [SerializeField]
        private string conn_x_Neg = "";
        [SerializeField]
        private string conn_x_Pos = "";
        [SerializeField]
        private string conn_z_Neg = "";
        [SerializeField]
        private string conn_z_Pos = "";

        private Vector3 oldPos = Vector3.zero;

        List<GameObject> toDrawTo = new List<GameObject>();

        private InputReader inputReader = null;
        private Vector3Int oldObjectSize = Vector3Int.zero;

        private Transform[] instConnectorTransforms = new Transform[0];

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (oldObjectSize != objectSize)
            {
                if (inputReader == null)
                {
                    inputReader = FindObjectOfType<InputReader>();
                }
                oldObjectSize = objectSize;

                for (int i = 0; i < instConnectorTransforms.Length; i++)
                {
                    DestroyImmediate(instConnectorTransforms[i].gameObject);
                }

                List<Transform> newTransforms = new List<Transform>();
                for (int z = 0; z < objectSize.z; z++)
                {
                    for (int y = 0; y < objectSize.y; y++)
                    {
                        for (int x = 0; x < objectSize.x; x++)
                        {
                            if (x == 0 || y == 0 || z == 0 || x == objectSize.x - 1 || y == objectSize.y - 1 || z == objectSize.z - 1)
                            {
                                GameObject newGo = new GameObject("Connector");
                                newGo.transform.parent = transform;
                                newGo.AddComponent<ElementConnector>();

                                // Add connectors on the sides
                                newGo.transform.localPosition = inputReader.InputGridSize * 0.5f + new Vector3()
                            }

                        }
                    }
                }

                instConnectorTransforms = newTransforms.ToArray();
            }

            /*if (oldPos != transform.position)
            {
                InputElement[] allEls = GameObject.FindObjectsOfType<InputElement>();

                toDrawTo = new List<GameObject>();

                for (int i = 0; i < allEls.Length; i++)
                {
                    if (allEls[i].transform.GetInstanceID() != transform.GetInstanceID() && Vector3.Distance(allEls[i].transform.position, transform.position) <= drawLinesDistance)
                    {
                        toDrawTo.Add(allEls[i].gameObject);
                    }
                }
            }


            if (Selection.activeGameObject != null && Selection.activeGameObject.transform.GetInstanceID() == transform.GetInstanceID())
            {
                for (int i = 0; i < toDrawTo.Count; i++)
                {
                    Debug.DrawLine(transform.position, toDrawTo[i].transform.position);
                }
            }
            else
            {
                toDrawTo = new List<GameObject>();
            }*/
        }


        private void OnDrawGizmos()
        {
            if (inputReader == null)
            {
                inputReader = FindObjectOfType<InputReader>();
            }


            Vector3 midPoint = new Vector3( objectSize.x * 0.5f * inputReader.InputGridSize.x,
                                            objectSize.y * 0.5f * inputReader.InputGridSize.y,
                                            objectSize.z * 0.5f * inputReader.InputGridSize.z);

            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawCube(transform.position + transform.rotation * midPoint, midPoint * 2f);
        }
    }

}