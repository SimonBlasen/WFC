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
        private const int resolutionNormalDivider = 8;

        private List<MeshConnPattern> meshConnPatterns = new List<MeshConnPattern>();

        private Transform pivot;

        public MeshAnalyzer(Vector3 gridSize)
        {
            this.gridSize = gridSize;

            GameObject pivotObject = new GameObject("Pivot Transform");
            pivot = pivotObject.transform;
        }

        public AnalyzedMesh AnalyzeMesh(GameObject objectWithMesh, bool visualize = false)
        {
            MeshFilter[] mf = objectWithMesh.GetComponentsInChildren<MeshFilter>();

            pivot.position = objectWithMesh.transform.position;
            pivot.rotation = objectWithMesh.transform.rotation;
            pivot.localScale = gridSize * 0.5f;

            List<Vector3Int> vertices = new List<Vector3Int>();
            List<Vector3Int> normals = new List<Vector3Int>();
            List<int> normalsCount = new List<int>();

            for (int filter = 0; filter < mf.Length; filter++)
            {
                Debug.Log("Analyzing " + mf[filter].transform.parent.parent.gameObject.name);

                Vector3[] localPosses = mf[filter].sharedMesh.vertices;
                Vector3[] localNormals = mf[filter].sharedMesh.normals;

                for (int i = 0; i < localPosses.Length; i++)
                {
                    Vector3 pivotPos = pivot.InverseTransformPoint(mf[filter].transform.TransformPoint(localPosses[i]));
                    Vector3 pivotNormal = pivot.InverseTransformDirection(mf[filter].transform.TransformDirection(localNormals[i]));

                    Vector3Int transformedPos = new Vector3Int(Mathf.RoundToInt((pivotPos * resolutionDividere).x), Mathf.RoundToInt((pivotPos * resolutionDividere).y), Mathf.RoundToInt((pivotPos * resolutionDividere).z));
                    Vector3Int transformedNormal = new Vector3Int(Mathf.RoundToInt((pivotNormal * resolutionNormalDivider).x), Mathf.RoundToInt((pivotNormal * resolutionNormalDivider).y), Mathf.RoundToInt((pivotNormal * resolutionNormalDivider).z));

                    if (vertices.Contains(transformedPos) == false)
                    {
                        vertices.Add(transformedPos);
                        normals.Add(transformedNormal);
                        normalsCount.Add(1);
                    }
                    else
                    {
                        for (int j = 0; j < vertices.Count; j++)
                        {
                            if (vertices[j] == transformedPos)
                            {
                                normals[j] = normals[j] + transformedNormal;
                                normalsCount[j] = normalsCount[j] + 1;
                            }
                        }
                    }
                }
            }


            bool hasError = false;
            for (int j = 0; j < vertices.Count; j++)
            {
                Vector3 normalFloat = new Vector3(normals[j].x, normals[j].y, normals[j].z) * (1f / normalsCount[j]);

                normals[j] = new Vector3Int(Mathf.RoundToInt(normalFloat.x), Mathf.RoundToInt(normalFloat.y), Mathf.RoundToInt(normalFloat.z));

                if (vertices[j].x < -resolutionDividere || vertices[j].x > resolutionDividere
                    || vertices[j].y < -resolutionDividere || vertices[j].y > resolutionDividere
                    || vertices[j].z < -resolutionDividere || vertices[j].z > resolutionDividere)
                {
                    hasError = true;
                    Debug.LogError("Vertices outside of bounds", objectWithMesh);

                    break;
                }
            }


            AnalyzedMesh analyzedMesh = null;
            if (!hasError)
            {
                analyzedMesh = new AnalyzedMesh(vertices.ToArray(), normals.ToArray());
            }


            if (visualize)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("PrefabDebugCube"));

                    go.transform.position = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z) * (1f / resolutionDividere);

                    go.transform.GetChild(0).forward = normals[i];
                }
            }
            


            tempDebugCounter++;

            return analyzedMesh;
        }
    }

}