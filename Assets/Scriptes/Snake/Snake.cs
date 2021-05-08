using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SnakeSkeleton))]
[RequireComponent(typeof(SnakeBoneMovement))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField] private Track _track;
    [SerializeField] private Transform _target;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private float _distanceCovered;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
    }

    private void Start()
    {
        _snakeBoneMovement.Init(_snakeSkeleton);
    }

    private void Update()
    {
        if (_distanceCovered <= 0.99f)
            Move();

        if (Input.GetKeyDown(KeyCode.Space))
            AddBoneInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveBoneFromTail();
    }

    private void Move()
    {
        _distanceCovered += 1 / _speedTime * Time.deltaTime;

        if (_target != null)
            _target.position = _track.GetPosition(_distanceCovered);

        _snakeBoneMovement.Move(_track, _distanceCovered, _distanceBetweenSegments);
    }

    private void AddBoneInTail()
    {
        _snakeSkeleton.AddBoneInTail();
    }

    private void RemoveBoneFromTail()
    {
        _snakeSkeleton.RemoveBoneFromTail();
    }
}
