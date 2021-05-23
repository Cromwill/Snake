using System;
using System.Linq;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
[RequireComponent(typeof(SnakeBoneMovement))]
[RequireComponent(typeof(SnakeBoneStretching))]
public class Snake : MonoBehaviour, IMoveable
{
    [SerializeField] private float _maxSpeedTime;
    [SerializeField] private float _distanceBetweenSegments = 1f;
    [SerializeField] private Animator _armatureAnimator;

    public event Action StartMoving;
    public event Action<float> Moving;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private SnakeBoneStretching _boneStretching;
    private Track _track;
    private FinishPath _finish;
    private float _distanceCovered;
    private float _finishDistanceCovered;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _speedRate;
    private bool _isMoving;

    public Transform HeadTransform => _snakeSkeleton.Head.transform;
    public Track Track => _track;
    public float DistanceCovered => _distanceCovered;
    public float NormalizeDistanceCovered => _distanceCovered / _track.DistanceLength;
    public float BoneDistance => _distanceBetweenSegments;
    public float MaxSpeed => _maxSpeedTime;
    public float CurrentSpeed => _currentSpeed;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
        _boneStretching = GetComponent<SnakeBoneStretching>();
    }

    private void OnEnable()
    {
        _snakeBoneMovement.PartiallyСrawled += OnPartiallyCrawled;
        _snakeBoneMovement.FullСrawled += OnFullCrawled;
    }

    private void OnDisable()
    {
        _snakeBoneMovement.PartiallyСrawled -= OnPartiallyCrawled;
        _snakeBoneMovement.FullСrawled -= OnFullCrawled;
    }

    private void OnFullCrawled()
    {
        enabled = false;
    }

    private void OnPartiallyCrawled(float distance)
    {
        enabled = false;
    }

    public void Init(Track track, FinishPath finish)
    {
        _track = track;
        _finish = finish;

        _distanceCovered = _snakeSkeleton.MinLength * _distanceBetweenSegments;
        _snakeBoneMovement.Init(_snakeSkeleton, _track, _finish);
    }

    private void Start()
    {
        _currentSpeed = 0;
        _speedRate = 1f;
        _snakeBoneMovement.Init(_snakeSkeleton, _track, _finish);
        _boneStretching.Init(_snakeSkeleton.Bones.Count(), _distanceBetweenSegments, _maxSpeedTime);

        OnStart();
    }

    protected virtual void OnStart() { }

    private void Update()
    {
        if (_distanceCovered < _track.DistanceLength)
            Move();
        else FinishMove();

        if (Input.GetKeyDown(KeyCode.A))
            AddBoneInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveBoneFromTail();
    }

    private void Move()
    {
        _distanceCovered = Mathf.MoveTowards(_distanceCovered, _track.DistanceLength, _currentSpeed * _speedRate * Time.deltaTime);
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, 14f * Time.deltaTime);

        _snakeBoneMovement.Move(_distanceCovered, _boneStretching.Distances);
        Moving?.Invoke(_distanceCovered);
    }

    private void FinishMove()
    {
        _currentSpeed = 0;
        _finishDistanceCovered = Mathf.MoveTowards(_finishDistanceCovered, _finish.DistanceLength, _maxSpeedTime * Time.deltaTime);

        _snakeBoneMovement.MoveFinish(_finishDistanceCovered, _distanceBetweenSegments);
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
        _targetSpeed = _maxSpeedTime;
        _isMoving = true;
        _armatureAnimator.SetBool("IsMoving", _isMoving);

        _boneStretching.StartStretching();

        StartMoving?.Invoke();
    }

    public virtual void EndMove()
    {
        _targetSpeed = 0;
        _isMoving = false;
        _armatureAnimator.SetBool("IsMoving", _isMoving);

        _boneStretching.StopStretching();
    }
}
