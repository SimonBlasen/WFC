using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzedMesh
{
    private int _yRotIndex = 0;
    private bool _mirrored = false;
    private Vector3Int[] _vertices;
    private Vector3Int[] _normals;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertices"> An unsorted list of all vertices, normalized inside of resolutionDivider</param>
    /// <param name="normals"></param>
    public AnalyzedMesh(Vector3Int[] vertices, Vector3Int[] normals)
    {
        _vertices = vertices;
        _normals = normals;
    }


    public bool IsEqualTo(AnalyzedMesh other)
    {
        if (ReferenceEquals(other, null))
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (this.GetType() != other.GetType())
        {
            return false;
        }

        if (_vertices.Length != other._vertices.Length)
        {
            return false;
        }

        for (int i = 0; i < _vertices.Length; i++)
        {
            bool foundAnEqual = false;
            for (int j = 0; j < _vertices.Length; j++)
            {
                if (_vertices[i] == other._vertices[j]
                    && Utils.AreNormalsEqual(_normals[i], other._normals[j]))
                {
                    foundAnEqual = true;
                    break;
                }
            }

            if (!foundAnEqual)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsMirroredDifferent
    {
        get; protected set;
    }


    public Quaternion LocalRotation
    {
        get
        {
            return Quaternion.Euler(0f, _yRotIndex * 90f, 0f);
        }
    }

    public Vector3 LocalScale
    {
        get
        {
            return new Vector3(_mirrored ? -1f : 1f, 1f, 1f);
        }
    }
}