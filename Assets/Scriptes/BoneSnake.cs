using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class BoneSnake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField] private Transform _armature;
    [SerializeField] private Track _track;
    [SerializeField] private Transform _testPoint;

    private List<Transform> _nodes;
    private float _distanceCovered;
    private int _minLength = 3;
    private int _currentLength;

    private void Start()
    {
        _currentLength = _minLength;
        _distanceCovered = 0;

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

        if (Input.GetKeyDown(KeyCode.Space))
            AddNodeInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveNodeFromTail();
    }

    private void Move()
    {
        _distanceCovered += 1 / _speedTime * Time.deltaTime;

        float scaleKf = 1f;
        for (int i = 0; i < _nodes.Count; i++)
        {
            var distance = _distanceCovered - i * _distanceBetweenSegments * 0.01f;
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

            if (i > _currentLength)
                _nodes[i].localScale = Vector3.zero;
            else
            {
                var scaleDistance = Vector3.Distance(_nodes[i].position, _testPoint.position);
                if (scaleDistance < 1.5f)
                {
                    _nodes[i].localScale = Vector3.one * (3f - scaleDistance);
                    scaleKf = 1f / (3f - scaleDistance);
                }
                else
                {
                    _nodes[i].localScale = Vector3.one * scaleKf;
                    scaleKf = 1f;
                }
            }
        }
    }

    private void AddNodeInTail()
    {
        _currentLength++;
    }

    private void RemoveNodeFromTail()
    {
        if (_currentLength > _minLength)
            _currentLength--;
    }
}
