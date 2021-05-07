using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SnakeSkeleton))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(0.01f, 1f)] private float _distanceBetweenSegments = 0.01f;
    [SerializeField] private Track _track;

    private SnakeSkeleton _snakeSkeleton;
    private float _distanceCovered;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
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

        for (int i = 0; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = _distanceCovered - i * _distanceBetweenSegments * 0.01f;
            if (distance < 0)
                continue;
            _snakeSkeleton.ActiveBones[i].Position = _track.GetPosition(distance);

            Vector3 forwardVector;
            if (i == 0)
                forwardVector = _snakeSkeleton.ActiveBones[i].Position - _snakeSkeleton.ActiveBones[i + 1].Position;
            else
                forwardVector = _snakeSkeleton.ActiveBones[i - 1].Position - _snakeSkeleton.ActiveBones[i].Position;

            _snakeSkeleton.ActiveBones[i].LookRotation(forwardVector);
        }
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
