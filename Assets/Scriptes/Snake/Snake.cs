using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
[RequireComponent(typeof(SnakeBoneMovement))]
public class Snake : MonoBehaviour, IMoveable
{
    [SerializeField] private float _maxSpeedTime;
    [SerializeField] private float _distanceBetweenSegments = 1f;
    [SerializeField] private Animator _armatureAnimator;
    [SerializeField] private float _deltaSegment;

    public event Action<float> Moving;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private Track _track;
    private FinishPath _finish;
    private GameObject _tapToPlayView;
    private float _distanceCovered;
    private float _finishDistanceCovered;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _speedRate;
    private bool _isMoving;
    private float _currentDistanceBetweenSegments;
    private Coroutine _acceleration;

    public Transform HeadTransform => _snakeSkeleton.Head.transform;
    public Track Track => _track;
    public float DistanceCovered => _distanceCovered;
    public float NormalizeDistanceCovered => _distanceCovered / _track.DistanceLength;
    public float BoneDistance => _distanceBetweenSegments;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
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

    public void Init(Track track, FinishPath finish, GameObject tapToPlay)
    {
        _track = track;
        _finish = finish;
        _tapToPlayView = tapToPlay;

        _distanceCovered = _snakeSkeleton.MinLength * _distanceBetweenSegments;
        _snakeBoneMovement.Init(_snakeSkeleton, _track, _finish);
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
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, 4f * Time.deltaTime);

        _snakeBoneMovement.Move(_distanceCovered, _currentDistanceBetweenSegments);
        Moving?.Invoke(_distanceCovered);
    }

    private void FinishMove()
    {
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

    private IEnumerator Acceleration(int direction)
    {
        float targetDistance = direction > 0 ? _distanceBetweenSegments + _deltaSegment : _distanceBetweenSegments;

        while (true)
        {
            _currentDistanceBetweenSegments = _currentDistanceBetweenSegments + (_deltaSegment * Time.deltaTime * direction);

            if (direction > 0 && _currentDistanceBetweenSegments >= _distanceBetweenSegments + _deltaSegment)
            {
                _currentDistanceBetweenSegments = _distanceBetweenSegments + _deltaSegment;
                break;
            }
            else if (direction < 0 && _currentDistanceBetweenSegments <= _distanceBetweenSegments)
            {
                _currentDistanceBetweenSegments = _distanceBetweenSegments;
                break;
            }

            yield return null;
        }
    }

    public void SetSpeedRate(float speedRate)
    {
        _speedRate = speedRate;
    }

    public virtual void StartMove()
    {
        if (_tapToPlayView != null && _tapToPlayView.activeSelf)
            _tapToPlayView.SetActive(false);

        if (!_isMoving)
        {
            if (_acceleration != null)
                StopCoroutine(_acceleration);

            _acceleration = StartCoroutine(Acceleration(1));
        }

        _targetSpeed = _maxSpeedTime;
        _isMoving = true;
        _armatureAnimator.SetBool("IsMoving", _isMoving);


    }

    public virtual void EndMove()
    {
        if (_isMoving)
        {
            if (_acceleration != null)
                StopCoroutine(_acceleration);

            _acceleration = StartCoroutine(Acceleration(-1));
        }


        _targetSpeed = 0;
        _isMoving = false;
        _armatureAnimator.SetBool("IsMoving", _isMoving);
    }
}
