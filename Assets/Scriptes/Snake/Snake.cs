﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
[RequireComponent(typeof(SnakeBoneMovement))]
public class Snake : MonoBehaviour, IMoveable
{
    [SerializeField] private float _defaultSpeedTime;
    [SerializeField] private float _maxSpeedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField, Range(0.01f, 1f)] private float _segmentLengthening = 0.01f;
    [SerializeField] private Track _track;
    [SerializeField] private Pole _pole;
    [SerializeField] private int _trackIndex;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _tapToPlayView;
    [SerializeField] private Animator _armatureAnimator;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private float _distanceCovered;
    private float _poleDistanceCovered;
    private Direction _lengtheningDirection;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _speedRate;
    private float _currentDistanceBetweenSegments;
    private bool _isMoving;

    public Transform HeadTransform => _snakeSkeleton.Head.transform;
    public Track Track => _track;
    public float DistanceCovered => _distanceCovered;
    public int TrackIndex => _trackIndex;
    public float BoneDistance => _distanceBetweenSegments;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
        _lengtheningDirection = Direction.Right;
        _currentDistanceBetweenSegments = _distanceBetweenSegments;
    }

    private void Start()
    {
        _currentSpeed = 0;
        _speedRate = 1f;
        _snakeBoneMovement.Init(_snakeSkeleton);

        OnStart();
    }

    protected virtual void OnStart() { }

    private void Update()
    {
        if (_distanceCovered <= 0.99f)
            Move();
        else
            _distanceCovered = 0f;
        //else if (_poleDistanceCovered <= 0.95f)
        //    MovePole();
        

        if (Input.GetKeyDown(KeyCode.A))
            AddBoneInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveBoneFromTail();
    }

    private void Move()
    {
        _distanceCovered = Mathf.MoveTowards(_distanceCovered, 1f, _currentSpeed * _speedRate * Time.deltaTime);
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, 4f * Time.deltaTime);

        if (_target != null)
        {
            _target.position = _track.GetPositionByIndex(_distanceCovered, _trackIndex);
            //float delta = 0.005f;
            //if (_distanceCovered > delta)
            //{
            //    var nextPosition = _track.GetPositionByIndex(_distanceCovered - delta, _trackIndex);
            //    _target.rotation = Quaternion.LookRotation(_target.position - nextPosition, Vector3.up);
            //}
        }

        if (_isMoving)
        {
            _currentDistanceBetweenSegments += _segmentLengthening * 2.0f * Time.deltaTime * (float)_lengtheningDirection;
            CheckDirection();
        }

        _snakeBoneMovement.Move(_track, _trackIndex, _distanceCovered, _distanceBetweenSegments);
    }

    private void MovePole()
    {
        _poleDistanceCovered = Mathf.MoveTowards(_poleDistanceCovered, 1f, _currentSpeed * _speedRate * 4f * Time.deltaTime);
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, 4f * Time.deltaTime);

        _snakeSkeleton.ActiveBones[0].Position = _pole.GetPosition(_poleDistanceCovered);

        var targetY = _snakeSkeleton.ActiveBones[0].Position.y;
        _target.position = new Vector3(_target.position.x, targetY, _target.position.z);

        if (_poleDistanceCovered < float.Epsilon)
            return;

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _pole.GetPosition(_poleDistanceCovered - 0.02f);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);
        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = _poleDistanceCovered - i * 0.02f;
            if (distance < 0)
                break;

            var trackPoint = _pole.GetPosition(distance);
            var currentBone = _snakeSkeleton.ActiveBones[i];
            currentBone.Position = trackPoint;

            forwardVector = _snakeSkeleton.ActiveBones[i - 1].Position - _snakeSkeleton.ActiveBones[i].Position;

            _snakeSkeleton.ActiveBones[i].LookRotation(forwardVector);
        }
    }

    private void AddBoneInTail()
    {
        _snakeSkeleton.AddBoneInTailSmoothly();
    }

    private void RemoveBoneFromTail()
    {
        _snakeSkeleton.RemoveBoneFromTail();
    }

    private void CheckDirection()
    {
        if (_lengtheningDirection == Direction.Right && _currentDistanceBetweenSegments >= _distanceBetweenSegments + _segmentLengthening)
            _lengtheningDirection = Direction.Left;
        else if (_lengtheningDirection == Direction.Left && _currentDistanceBetweenSegments <= _distanceBetweenSegments)
            _lengtheningDirection = Direction.Right;
    }

    public void SetSpeedRate(float speedRate)
    {
        _speedRate = speedRate;
    }

    public virtual void StartMove()
    {
        if (_tapToPlayView != null && _tapToPlayView.activeSelf)
            _tapToPlayView.SetActive(false);

        _targetSpeed = _maxSpeedTime;
        _isMoving = true;
        _armatureAnimator.SetBool("IsMoving", _isMoving);
        _currentDistanceBetweenSegments = _distanceBetweenSegments;
    }

    public virtual void EndMove()
    {
        _targetSpeed = 0;
        _isMoving = false;
        _armatureAnimator.SetBool("IsMoving", _isMoving);
    }
}
