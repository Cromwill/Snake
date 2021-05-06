using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(MeshFilter))]
public class MeshScaler : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _minDistance = 2f;

    private Mesh _mesh;
    private Vector3[] _baseVertices;

    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    public MeshVertex GetVertexWithScale(MeshVertex vertex) // не доделано
    {
        if (_targetPoint == null)
            return vertex;

        var distance = Vector3.Distance(transform.TransformPoint(vertex.position), _targetPoint.position);
        if (distance < _minDistance)
        {
            var scale = 1 + distance / _minDistance;
            var scaleVector = transform.TransformPoint(vertex.position) - _targetPoint.position;
            
            scaleVector = vertex.normal;
            scaleVector.Normalize();

            vertex.position += scaleVector * scale * 0.1f;
        }

        return vertex;
    }

    private Vector3 GetVertexWithScale(Vector3 vertexPosition)
    {
        if (_targetPoint == null)
            return vertexPosition;

        var distance = Vector3.Distance(transform.TransformPoint(vertexPosition), _targetPoint.position);
        if (distance < _minDistance)
        {
            var scale = 1 + _minDistance / distance;
            var scaleVector = transform.TransformPoint(vertexPosition) - _targetPoint.position;
            scaleVector.Normalize();
            
            vertexPosition += scaleVector * scale;
        }

        return vertexPosition;
    }

    private void Update()
    {
        //for (var i = 0; i < vertices.Length; i++)
        //{
        //    var vertex = _baseVertices[i];

        //    var distance = Vector3.Distance(transform.TransformPoint(vertex), _targetPoint.position);
        //    var scale = 1f;
        //    var scaleVector = Vector3.zero;
        //    if (distance < _minDistance)
        //    {
        //        scale = 1 + _minDistance - distance;
        //        scaleVector = transform.TransformPoint(vertex) - _targetPoint.position;
        //        scaleVector.Normalize();
        //    }

        //    vertex += scaleVector * scale * 0.001f;

        //    vertices[i] = vertex;
        //}

        //_mesh.vertices = vertices;

        //_mesh.RecalculateNormals();
        //_mesh.RecalculateBounds();
    }

    private void PrintMeshInfo()
    {
        Debug.Log("------------------------------------------------------");
        Debug.Log("Vertices: " + _mesh.vertices.Length);
        foreach (var vertices in _mesh.vertices)
            Debug.Log(vertices);

        Debug.Log("------------------------------------------------------");
        Debug.Log("Triangles: " + _mesh.triangles.Length);
        foreach (var triangle in _mesh.triangles)
            Debug.Log(triangle);

        Debug.Log("------------------------------------------------------");
        Debug.Log("UVs: " + _mesh.uv.Length);
        foreach (var uv in _mesh.uv)
            Debug.Log(uv);
    }
}
