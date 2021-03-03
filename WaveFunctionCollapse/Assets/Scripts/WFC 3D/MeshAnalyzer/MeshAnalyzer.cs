using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WFC3D
{
    public class MeshAnalyzer
    {
        private int tempDebugCounter = 0;

        private Vector3 gridSize = new Vector3(1f, 1f, 1f);

        private const int resolutionDividere = 16;

        private List<MeshConnPattern> meshConnPatterns = new List<MeshConnPattern>();

        private Transform pivot;

        public MeshAnalyzer(Vector3 gridSize)
        {
            this.gridSize = gridSize;

            GameObject pivotObject = new GameObject("Pivot Transform");
            pivot = pivotObject.transform;
        }

        public MeshConnPattern AnalyzeFace(GameObject objectWithMesh)
        {
            MeshFilter[] mf = objectWithMesh.GetComponentsInChildren<MeshFilter>();

            pivot.position = objectWithMesh.transform.position;
            pivot.rotation = objectWithMesh.transform.rotation;
            pivot.localScale = gridSize * 0.5f;

            List<Vector3Int> vertices = new List<Vector3Int>();

            for (int filter = 0; filter < mf.Length; filter++)
            {
                Debug.Log("Analyzing " + mf[filter].transform.parent.parent.gameObject.name);

                Vector3[] localPosses = mf[filter].mesh.vertices;

                for (int i = 0; i < localPosses.Length; i++)
                {
                    Vector3 pivotPos = pivot.InverseTransformPoint(mf[filter].transform.TransformPoint(localPosses[i]));

                    Vector3Int transformedPos = new Vector3Int(Mathf.RoundToInt((pivotPos * resolutionDividere).x), Mathf.RoundToInt((pivotPos * resolutionDividere).y), Mathf.RoundToInt((pivotPos * resolutionDividere).z));

                    if (vertices.Contains(transformedPos) == false)
                    {
                        vertices.Add(transformedPos);
                    }
                }
            }

            /*
            if (tempDebugCounter == 0)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("PrefabDebugCube"));

                    go.transform.position = vertices[i];
                }
            }
            */


            tempDebugCounter++;

            return null;
        }
    }

}