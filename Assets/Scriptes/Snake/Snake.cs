using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
[RequireComponent(typeof(SnakeBoneMovement))]
public class Snake : MonoBehaviour, IMoveable
{
    [SerializeField] private float _maxSpeedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField] private Track _track;
    [SerializeField] private FinishPath _finish;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _tapToPlayView;
    [SerializeField] private Animator _armatureAnimator;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private float _distanceCovered;
    private float _finishDistanceCovered;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _speedRate;
    private bool _isMoving;

    public Transform HeadTransform => _snakeSkeleton.Head.transform;
    public Track Track => _track;
    public float DistanceCovered => _distanceCovered;
    public float BoneDistance => _distanceBetweenSegments;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
    }

    private void Start()
    {
        _currentSpeed = 0;
        _speedRate = 1f;
        _snakeBoneMovement.Init(_snakeSkeleton, _track, _finish);

        OnStart();
    }

    protected virtual void OnStart() { }

    private void Update()
    {
        if (_distanceCovered < .99f)
            Move();
        else if (_finishDistanceCovered < _finish.DistanceLength)
            FinishMove();


        if (Input.GetKeyDown(KeyCode.A))
            AddBoneInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveBoneFromTail();
    }

    private void Move()
    {
        _distanceCovered = Mathf.MoveTowards(_distanceCovered, 1f, _currentSpeed / _track.DistanceLength * _speedRate * Time.deltaTime);
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, 4f * Time.deltaTime);

        if (_target != null)
        {
            _target.position = _track.GetPosition(_distanceCovered);
        }

        _snakeBoneMovement.Move(_distanceCovered, _distanceBetweenSegments);
    }

    private void FinishMove()
    {
        _finishDistanceCovered = Mathf.MoveTowards(_finishDistanceCovered, _finish.DistanceLength, _maxSpeedTime * Time.deltaTime);

        _snakeSkeleton.ActiveBones[0].Position = _finish.GetPositionByDistance(_finishDistanceCovered);

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _finish.GetPositionByDistance(_finishDistanceCovered - 0.01f);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = _finishDistanceCovered - i * _distanceBetweenSegments;
            if (distance < 0)
            {
                var shiftParam = _distanceCovered - (_distanceBetweenSegments / _track.DistanceLength) + distance / _track.DistanceLength;
                _snakeBoneMovement.MoveFrom(i, shiftParam, _distanceBetweenSegments);
                break;
            }

            var trackPoint = _finish.GetPositionByDistance(distance);
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
    }

    public virtual void EndMove()
    {
        _targetSpeed = 0;
        _isMoving = false;
        _armatureAnimator.SetBool("IsMoving", _isMoving);
    }
}
