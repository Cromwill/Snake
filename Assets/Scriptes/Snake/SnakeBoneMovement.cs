using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SnakeBoneMovement : MonoBehaviour
{
    [SerializeField] private bool _curveMovement;
    [SerializeField] private float _curveAmplitude;
    [SerializeField] private float _curveSpeed;

    private SnakeSkeleton _snakeSkeleton;
    private Track _track;
    private FinishPath _finishPath;

    public void Init(SnakeSkeleton snakeSkeleton, Track track, FinishPath finishPath)
    {
        _snakeSkeleton = snakeSkeleton;
        _track = track;
        _finishPath = finishPath;
    }

    public void Move(float headDistance, float boneDistance)
    {
        _snakeSkeleton.ActiveBones[0].Position = _track.GetPosition(headDistance);

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _track.GetPosition(headDistance - boneDistance / _track.DistanceLength);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance / _track.DistanceLength;
            if (distance < 0)
                break;

            MoveBoneOnTrack(i, distance);
        }
    }

    public void MoveFrom(int fromBoneIndex, float headDistance, float boneDistance)
    {
        for (int i = fromBoneIndex; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - (i - fromBoneIndex) * boneDistance / _track.DistanceLength;
            if (distance < 0)
                continue;

            MoveBoneOnTrack(i, distance);
        }
    }

    public void MoveFinish(float headDistance, float boneDistance)
    {
        _snakeSkeleton.ActiveBones[0].Position = _finishPath.GetPositionByDistance(headDistance);

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _finishPath.GetPositionByDistance(headDistance - 0.01f);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance;
            if (distance < 0)
            {
                var shiftParam = 1f - (boneDistance / _track.DistanceLength) + distance / _track.DistanceLength;
                MoveFrom(i, shiftParam, boneDistance);
                break;
            }

            var trackPoint = _finishPath.GetPositionByDistance(distance);
            var currentBone = _snakeSkeleton.ActiveBones[i];
            currentBone.Position = trackPoint;

            forwardVector = _snakeSkeleton.ActiveBones[i - 1].Position - _snakeSkeleton.ActiveBones[i].Position;

            _snakeSkeleton.ActiveBones[i].LookRotation(forwardVector);
        }
    }

    private void MoveBoneOnTrack(int boneIndex, float boneDistance)
    {
        var trackPoint = _track.GetPosition(boneDistance);
        var currentBone = _snakeSkeleton.ActiveBones[boneIndex];
        currentBone.Position = trackPoint;

        if (_curveMovement)
        {
            var delta = boneDistance * _track.DistanceLength;
            var amplitude = _curveAmplitude * Mathf.Sqrt(boneIndex * 2f);
            currentBone.Position += currentBone.transform.right * amplitude * Mathf.Sin(delta * _curveSpeed);
        }

        var forwardVector = _snakeSkeleton.ActiveBones[boneIndex - 1].Position - _snakeSkeleton.ActiveBones[boneIndex].Position;

        _snakeSkeleton.ActiveBones[boneIndex].LookRotation(forwardVector);
    }
}
