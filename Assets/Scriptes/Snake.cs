using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField] private Track _track;

    private Spline _selfSpline;
    private MeshFilter _meshFilter;
    private Transform _selfTransform;
    private float _distanceCovered;

    private void Start()
    {
        _distanceCovered = 0;
        _selfSpline = GetComponent<Spline>();
        _meshFilter = GetComponent<MeshFilter>();
        _selfTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (_distanceCovered <= 0.99f)
            Move();

        if (Input.GetKeyDown(KeyCode.Space))
            AddNodeInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveNodeFromTail();
    }

    private void Move()
    {
        _distanceCovered += 1 / _speedTime * Time.deltaTime;

        for (int i = 0; i < _selfSpline.nodes.Count; i++)
        {
            var distance = _distanceCovered - i / 100f;
            if (distance < 0)
                continue;
            _selfSpline.nodes[i].Position = _track.GetPosition(distance) - transform.position;
        }
    }

    private void AddNodeInTail()
    {
        var tailNode = _selfSpline.nodes[_selfSpline.nodes.Count - 1];
        SplineNode newNode = new SplineNode(tailNode.Direction, tailNode.Direction + tailNode.Direction - tailNode.Position);
        _selfSpline.AddNode(newNode);
    }

    private void RemoveNodeFromTail()
    {
        if (_selfSpline.nodes.Count <= 2)
            return;

        var tailNode = _selfSpline.nodes[_selfSpline.nodes.Count - 1];
        _selfSpline.RemoveNode(tailNode);
    }
}
