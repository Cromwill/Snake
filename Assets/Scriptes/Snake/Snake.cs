using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SnakeSkeleton))]
[RequireComponent(typeof(SnakeBoneMovement))]
public class Snake : MonoBehaviour, IMoveable
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField] private Track _track;
    [SerializeField] private Transform _target;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private float _distanceCovered;
    private float _currentSpeed;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
    }

    private void Start()
    {
        _snakeBoneMovement.Init(_snakeSkeleton);
        _currentSpeed = 0f;
    }

    private void Update()
    {
        if (_distanceCovered <= 0.99f)
            Move();

        if (Input.GetKeyDown(KeyCode.A))
            AddBoneInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveBoneFromTail();
    }

    private void Move()
    {
        if (_currentSpeed == 0)
            return;

        _distanceCovered += 1 / _currentSpeed * Time.deltaTime;

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

    public void StartMove()
    {
        _currentSpeed = _speedTime;
    }

    public void EndMove()
    {
        _currentSpeed = 0f;
    }
}
