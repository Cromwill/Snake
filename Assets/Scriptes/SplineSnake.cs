﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class SplineSnake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField] private Track _track;

    private Spline _selfSpline;
    private float _distanceCovered;

    private void Start()
    {
        _distanceCovered = 0;
        _selfSpline = GetComponent<Spline>();
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
            var distance = _distanceCovered - i * (_distanceBetweenSegments);
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