﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SnakeSkeleton))]
[RequireComponent(typeof(SnakeBoneMovement))]
public class Snake : MonoBehaviour, IMoveable
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField, Range(0.01f, 1f)] private float _segmentLengthening = 0.01f;
    [SerializeField] private Track _track;
    [SerializeField] private int _trackIndex;
    [SerializeField] private Transform _target;
    [SerializeField] private SkinMaterialColorShaker _materialShaker;
    [SerializeField] private GameObject _tapToPlayView;
    [SerializeField] private Animator _armatureAnimator;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private float _distanceCovered;
    private float _currentSpeed;
    private float _targetSpeed;
    private Direction _lengtheningDirection;
    private float _currentDistanceBetweenSegments;
    private bool _isMoving;

    public Transform HeadTransform => _snakeSkeleton.Head.transform;
    public float DistanceCovered => _distanceCovered;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
        _lengtheningDirection = Direction.Right;
        _currentDistanceBetweenSegments = _distanceBetweenSegments;
    }

    private void OnEnable()
    {
        _snakeSkeleton.Head.ObstacleEntered += OnObstacleEntered;
    }

    private void OnDisable()
    {
        _snakeSkeleton.Head.ObstacleEntered -= OnObstacleEntered;
    }

    private void Start()
    {
        _snakeBoneMovement.Init(_snakeSkeleton);
        _currentSpeed = 0f;

        OnStart();
    }

    protected virtual void OnStart() { }

    private void Update()
    {
        if (_distanceCovered <= 0.99f)
            Move();
        else
            _distanceCovered = 0;

        if (Input.GetKeyDown(KeyCode.A))
            AddBoneInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveBoneFromTail();
    }

    private void Move()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, 5f * Time.deltaTime);
        _distanceCovered = Mathf.MoveTowards(_distanceCovered, 1f, _currentSpeed * Time.deltaTime);

        if (_target != null)
            _target.position = _track.GetPositionByIndex(_distanceCovered, _trackIndex);
      
        if (_isMoving)
        {
            _currentDistanceBetweenSegments += _segmentLengthening * 2.0f * Time.deltaTime * (float)_lengtheningDirection;
            CheckDirection();
        }

        _snakeBoneMovement.Move(_track, _trackIndex, _distanceCovered, _currentDistanceBetweenSegments);
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        _materialShaker.Shake(Color.red, 3);
    }

    private void AddBoneInTail()
    {
        _snakeSkeleton.AddBoneInTail();
    }

    private void RemoveBoneFromTail()
    {
        _snakeSkeleton.RemoveBoneFromTail();
    }

    private void CheckDirection()
    {
        if (_lengtheningDirection == Direction.Right && _currentDistanceBetweenSegments >= _distanceBetweenSegments + _segmentLengthening)
            _lengtheningDirection = Direction.Left;
        else if(_lengtheningDirection == Direction.Left && _currentDistanceBetweenSegments <= _distanceBetweenSegments)
            _lengtheningDirection = Direction.Right;
    }

    public virtual void StartMove()
    {
        if (_tapToPlayView != null && _tapToPlayView.activeSelf)
            _tapToPlayView.SetActive(false);

        _targetSpeed = _speedTime;
        _isMoving = true;
        _armatureAnimator.Play("SnakeWalk");
        _armatureAnimator.SetBool("IsMoving", _isMoving);
        _currentDistanceBetweenSegments = _distanceBetweenSegments;
    }

    public virtual void EndMove()
    {
        _targetSpeed = 0f;
        _isMoving = false;
        _armatureAnimator.Play("Idle");
        _armatureAnimator.SetBool("IsMoving", _isMoving);

    }
}
