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
    [SerializeField] private Animator _armatureAnimator;

    public event Action StartMoving;
    public event Action<float> Moving;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private SnakeBoneStretching _boneStretching;
    private Track _track;
    private FinishPath _finish;
    private BonusFinish _bonusFinish;
    private float _distanceBetweenSegments = 1.5f;
    private float _distanceCovered;
    private float _finishDistanceCovered;
    private float _bonusPoleDistanceCovered;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _speedRate;
    private bool _isMoving = false;
    private bool _moveStarted = false;
    private float _acceleration;
    private Coroutine _accelerating;

    public Transform HeadTransform => _snakeSkeleton.Head.transform;
    public Track Track => _track;
    public float DistanceCovered => _distanceCovered;
    public float NormalizeDistanceCovered
    {
        get
        {
            if (_track)
                return _distanceCovered / _track.DistanceLength;
            return _bonusPoleDistanceCovered / _bonusFinish.DistanceLength;
        }
    }
    public float BoneDistance => _distanceBetweenSegments;
    public float MaxSpeed => _maxSpeedTime;
    public float CurrentSpeed => _currentSpeed;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
        _boneStretching = GetComponent<SnakeBoneStretching>();
        _acceleration = 1.0f;
    }

    private void OnEnable()
    {
        _snakeBoneMovement.PartiallyCrawled += OnPartiallyCrawled;
        _snakeBoneMovement.FullCrawled += OnFullCrawled;
    }

    private void OnDisable()
    {
        _snakeBoneMovement.PartiallyCrawled -= OnPartiallyCrawled;
        _snakeBoneMovement.FullCrawled -= OnFullCrawled;
    }

    private void OnFullCrawled()
    {
        enabled = false;
    }

    private void OnPartiallyCrawled(float distance)
    {
        enabled = false;
    }

    public void Init(Track track, FinishPath finish, BonusFinish bonusFinish)
    {
        _track = track;
        _finish = finish;
        _bonusFinish = bonusFinish;

        _distanceCovered = _snakeSkeleton.MinLength * _distanceBetweenSegments;
        _bonusPoleDistanceCovered = 10 * _distanceBetweenSegments;
        _snakeBoneMovement.Init(_snakeSkeleton, _track, _finish, _bonusFinish);

        if (bonusFinish)
            bonusFinish.Init(_snakeBoneMovement);
    }

    private void Start()
    {
        _currentSpeed = 0;
        _speedRate = 1f;
        _snakeBoneMovement.Init(_snakeSkeleton, _track, _finish, _bonusFinish);
        _boneStretching.Init(_snakeSkeleton.Bones.Count(), _distanceBetweenSegments, _maxSpeedTime);

        OnStart();
    }

    protected virtual void OnStart() { }

    private void Update()
    {
        if (_bonusFinish)
        {
            MoveBonusFinish();
        }
        else
        {
            if (_distanceCovered < _track.DistanceLength)
                Move();
            else FinishMove();
        }

        if (Input.GetKeyDown(KeyCode.A))
            AddBoneInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveBoneFromTail();
    }

    private void Move()
    {
        _distanceCovered = Mathf.MoveTowards(_distanceCovered, _track.DistanceLength, _currentSpeed * _acceleration * _speedRate * Time.deltaTime);
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

    private void MoveBonusFinish()
    {
        if (_bonusPoleDistanceCovered >= _bonusFinish.DistanceLength)
            return;

        if (_snakeSkeleton.ActiveBones.Count < 10)
            _snakeSkeleton.AddBoneInTail();

        if (_moveStarted)
            _bonusPoleDistanceCovered = Mathf.MoveTowards(_bonusPoleDistanceCovered, _bonusFinish.DistanceLength, _maxSpeedTime * Time.deltaTime);

        _snakeBoneMovement.MoveBonusFinish(_bonusPoleDistanceCovered, _distanceBetweenSegments);
    }

    private void AddBoneInTail()
    {
        _snakeSkeleton.AddBoneInTailSmoothly();
    }

    private void RemoveBoneFromTail()
    {
        _snakeSkeleton.RemoveBoneFromTail();
    }

    private IEnumerator Acceleration()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            _acceleration *= 1.1f;
        }
    }

    private void StopAcceleration()
    {
        _acceleration = 1.0f;
        if (_accelerating != null)
            StopCoroutine(_accelerating);
        _accelerating = null;
    }

    public void SetSpeedRate(float speedRate)
    {
        _speedRate = speedRate;
        StopAcceleration();
    }

    public virtual void StartMove()
    {
        _targetSpeed = _maxSpeedTime;
        _isMoving = true;
        _armatureAnimator.SetBool("IsMoving", _isMoving);

        if (_bonusFinish && _moveStarted)
            _bonusFinish.Jump(_bonusPoleDistanceCovered / _bonusFinish.DistanceLength);

        if (_accelerating == null)
            _accelerating = StartCoroutine(Acceleration());

        _boneStretching.StartStretching();

        StartMoving?.Invoke();
        _moveStarted = true;
    }

    public virtual void EndMove()
    {
        _targetSpeed = 0;
        _isMoving = false;
        _armatureAnimator.SetBool("IsMoving", _isMoving);
        StopAcceleration();
        _boneStretching.StopStretching();
    }
}
