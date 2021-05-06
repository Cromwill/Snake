using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class Snake1 : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(1, 100)] private float _distanceBetweenSegments = 1;
    [SerializeField] private Transform _armature;
    [SerializeField] private Track _track;

    private List<Transform> _nodes;
    private Transform _selfTransform;
    private float _distanceCovered;

    private void Start()
    {
        _distanceCovered = 0;
        _selfTransform = GetComponent<Transform>();

        _nodes = new List<Transform>();
        InitNodeList(_armature.GetChild(0));
    }

    private void InitNodeList(Transform currentNode)
    {
        _nodes.Add(currentNode);
        if (currentNode.childCount > 0)
            InitNodeList(currentNode.GetChild(0));
    }

    private void Update()
    {
        if (_distanceCovered <= 0.99f)
            Move();

        //if (Input.GetKeyDown(KeyCode.Space))
        //    AddNodeInTail();
        //if (Input.GetKeyDown(KeyCode.R))
        //    RemoveNodeFromTail();
    }

    private void Move()
    {
        _distanceCovered += 1 / _speedTime * Time.deltaTime;

        for (int i = 0; i < _nodes.Count; i++)
        {
            var distance = _distanceCovered - i / (100 - _distanceBetweenSegments) / 101;
            if (distance < 0)
                continue;
            _nodes[i].position = _track.GetPosition(distance);

            Vector3 forwardVector = Vector3.zero;
            if (i == 0)
                forwardVector = _nodes[i].position - _nodes[i + 1].position;
            else
                forwardVector = _nodes[i - 1].position - _nodes[i].position;

            var rotation = Quaternion.LookRotation(forwardVector, Vector3.up);
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x - 90f, rotation.eulerAngles.y, rotation.eulerAngles.z);
            _nodes[i].rotation = rotation;
        }
    }

    //private void AddNodeInTail()
    //{
    //    var tailNode = _selfSpline.nodes[_selfSpline.nodes.Count - 1];
    //    SplineNode newNode = new SplineNode(tailNode.Direction, tailNode.Direction + tailNode.Direction - tailNode.Position);
    //    _selfSpline.AddNode(newNode);
    //}

    //private void RemoveNodeFromTail()
    //{
    //    if (_selfSpline.nodes.Count <= 2)
    //        return;

    //    var tailNode = _selfSpline.nodes[_selfSpline.nodes.Count - 1];
    //    _selfSpline.RemoveNode(tailNode);
    //}
}
