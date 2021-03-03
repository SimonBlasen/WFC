using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public class MeshConnPattern
    {
        private List<Vector2Int> vertices = new List<Vector2Int>();

        public MeshConnPattern(Vector3Int[] allVertices, Vector3Int side)
        {
            // (1, 0, 0)  -> (0, 1, 1)
            // (0, -1, 0) -> (1, 0, 1)

            Vector3Int faceVector = new Vector3Int(1 - Mathf.Abs(side.x), 1 - Mathf.Abs(side.y), 1 - Mathf.Abs(side.z));

            // Init with first vertex
            int maxValue = allVertices[0].x * side.x + allVertices[0].y * side.y + allVertices[0].z * side.z;

            for (int i = 0; i < allVertices.Length; i++)
            {
                int vertValue = allVertices[i].x * side.x + allVertices[i].y * side.y + allVertices[i].z * side.z;

                if (vertValue > maxValue)
                {
                    maxValue = vertValue;
                }
            }

            List<Vector2Int> vectors2D = new List<Vector2Int>();

            // Find all vertices on face
            for (int i = 0; i < allVertices.Length; i++)
            {
                int vertValue = allVertices[i].x * side.x + allVertices[i].y * side.y + allVertices[i].z * side.z;

                if (vertValue == maxValue)
                {
                    vectors2D.Add(flatten(allVertices[i], side));
                }
            }


        }

        private Vector2Int flatten(Vector3Int vector, Vector3Int oneShotVector)
        {
            if (oneShotVector.x != 0)
            {
                return new Vector2Int(vector.y, vector.z);
            }
            else if (oneShotVector.y != 0)
            {
                return new Vector2Int(vector.x, vector.z);
            }
            else
            {
                return new Vector2Int(vector.x, vector.y);
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as MeshConnPattern);
        }

        public bool Equals(MeshConnPattern other)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            if (vertices.Count != other.vertices.Count)
            {
                return false;
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].x != other.vertices[i].x
                    || vertices[i].y != other.vertices[i].y)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0x0;
            for (int i = 0; i < vertices.Count; i++)
            {
                hash *= vertices[i].GetHashCode();
            }
            return hash;
        }

        public static bool operator ==(MeshConnPattern a, MeshConnPattern b)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(a, null))
            {
                if (Object.ReferenceEquals(b, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return a.Equals(b);
        }

        public static bool operator !=(MeshConnPattern a, MeshConnPattern b)
        {
            return !(a == b);
        }
    }

}