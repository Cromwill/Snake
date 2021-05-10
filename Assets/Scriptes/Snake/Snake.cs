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
    [SerializeField] private int _trackIndex;
    [SerializeField] private Transform _target;

    private SnakeSkeleton _snakeSkeleton;
    private SnakeBoneMovement _snakeBoneMovement;
    private float _distanceCovered;
    private float _currentSpeed;
    private float _targetSpeed;

    public Transform HeadTransform => _snakeSkeleton.Head.transform;
    public float DistanceCovered => _distanceCovered;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _snakeBoneMovement = GetComponent<SnakeBoneMovement>();
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
        
        _snakeBoneMovement.Move(_track, _trackIndex, _distanceCovered, _distanceBetweenSegments);
    }

    private void AddBoneInTail()
    {
        _snakeSkeleton.AddBoneInTail();
    }

    private void RemoveBoneFromTail()
    {
        _snakeSkeleton.RemoveBoneFromTail();
    }

    public virtual void StartMove()
    {
        _targetSpeed = _speedTime;
    }

    public virtual void EndMove()
    {
        _targetSpeed = 0f;
    }
}
