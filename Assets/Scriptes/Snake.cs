using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField] private Track _track;

    private MeshFilter _meshFilter;
    private Transform _selfTransform;
    private float _distanceCovered;


    private void Start()
    {
        _distanceCovered = 0;
        _selfTransform = GetComponent<Transform>();
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (_distanceCovered <= 1)
            Move();
    }

    private void Move()
    {
        _distanceCovered += 1 / _speedTime * Time.deltaTime;
        _selfTransform.position = _track.GetPosition(_distanceCovered);
    }
}
