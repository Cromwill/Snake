using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeBoneMovement : MonoBehaviour
{
    [SerializeField] private float _upShift = 0f;
    [SerializeField] private bool _curveMovement;
    [SerializeField] private float _curveAmplitude;
    [SerializeField] private float _curveSpeed;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _cameraForward;

    public Vector3 CameraTargetPosition => _cameraTarget.position;

    public event UnityAction<float> FinishDistanceCovered;
    public event UnityAction<float> PartiallyCrawled;
    public event UnityAction FullCrawled;
    public event UnityAction BonusPoleCrawled;

    private SnakeSkeleton _snakeSkeleton;
    private Track _track;
    private FinishPath _finishPath;
    private BonusFinish _bonusFinish;

    public void Init(SnakeSkeleton snakeSkeleton, Track track, FinishPath finishPath, BonusFinish bonusFinish)
    {
        _snakeSkeleton = snakeSkeleton;
        _track = track;
        _finishPath = finishPath;
        _bonusFinish = bonusFinish;
    }

    public void Move(float headDistance, float boneDistance)
    {
        _snakeSkeleton.ActiveBones[0].Position = _track.GetPositionByDistance(headDistance) + Vector3.up * _upShift;
        
        if (headDistance + 0.01f > _track.DistanceLength)
            return;

        var forwardVector = -_snakeSkeleton.ActiveBones[0].Position + _track.GetPositionByDistance(headDistance + 0.01f) + Vector3.up * _upShift;
        _snakeSkeleton.ActiveBones[0].Position += _snakeSkeleton.ActiveBones[0].transform.right * _curveAmplitude * Mathf.Sin(headDistance * _curveSpeed);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance;

            if (distance <= 0)
                break;

            MoveBoneOnTrack(i, distance, _upShift);
        }
    }

    public void Move(float headDistance, List<float> distances)
    {
        _snakeSkeleton.ActiveBones[0].Position = _track.GetPositionByDistance(headDistance) + Vector3.up * _upShift;

        float cameraDistance = headDistance + _cameraForward < _track.DistanceLength ? headDistance + _cameraForward : _track.DistanceLength;

        _cameraTarget.position = _track.GetPositionByDistance(cameraDistance) + Vector3.up * _upShift;

        if (headDistance + 0.01f > _track.DistanceLength)
            return;

        var forwardVector = -_snakeSkeleton.ActiveBones[0].Position + _track.GetPositionByDistance(headDistance + 0.01f) + Vector3.up * _upShift;
        _snakeSkeleton.ActiveBones[0].Position += _snakeSkeleton.ActiveBones[0].transform.right * _curveAmplitude * Mathf.Sin(headDistance * _curveSpeed);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        var distance = headDistance;
        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            distance -= distances[i];
            if (distance < 0)
                break;

            MoveBoneOnTrack(i, distance, _upShift);
        }
    }

    public void MoveFrom(int fromBoneIndex, float headDistance, float boneDistance)
    {
        for (int i = fromBoneIndex; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - (i - fromBoneIndex) * boneDistance;
            if (distance < 0)
                continue;

            MoveBoneOnTrack(i, distance);
        }
    }

    public void MoveFinish(float headDistance, float boneDistance)
    {
        _snakeSkeleton.ActiveBones[0].Position = _finishPath.GetPositionByDistance(headDistance);

        if (headDistance + 0.01f <= _finishPath.DistanceLength)
        {
            var forwardVector = _finishPath.GetPositionByDistance(headDistance + 0.01f) - _snakeSkeleton.ActiveBones[0].Position;
            _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);
            _snakeSkeleton.ActiveBones[0].Position -= _snakeSkeleton.ActiveBones[0].transform.right * 0.2f;
        }

        int boneIndex = 1;
        float distance = 0f;
        for (; boneIndex < _snakeSkeleton.ActiveBones.Count; boneIndex++)
        {
            distance = headDistance - boneIndex * boneDistance;
            if (distance < 0)
            {
                var shiftParam = _track.DistanceLength + distance;
                MoveFrom(boneIndex, shiftParam, boneDistance);
                break;
            }

            var trackPoint = _finishPath.GetPositionByDistance(distance);
            var currentBone = _snakeSkeleton.ActiveBones[boneIndex];
            currentBone.Position = trackPoint;

            var forwardVector = _finishPath.GetPositionByDistance(distance + 0.01f) - currentBone.Position;

            _snakeSkeleton.ActiveBones[boneIndex].LookRotation(forwardVector);
        }

        FinishDistanceCovered?.Invoke(headDistance);

        if (headDistance == _finishPath.DistanceLength)
            FullCrawled?.Invoke();
        else if (boneIndex == _snakeSkeleton.ActiveBones.Count && _finishPath.GetParameterByDistance(distance) > 1f)
            PartiallyCrawled?.Invoke(headDistance);
    }

    public void MoveBonusFinish(float headDistance, float boneDistance)
    {
        _snakeSkeleton.ActiveBones[0].Position = _bonusFinish.GetPositionByDistance(headDistance);

        if (headDistance + 0.01f <= _bonusFinish.DistanceLength)
        {
            var forwardVector = _bonusFinish.GetPositionByDistance(headDistance + 0.01f) - _snakeSkeleton.ActiveBones[0].Position;
            _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);
            _snakeSkeleton.ActiveBones[0].Position -= _snakeSkeleton.ActiveBones[0].transform.right * 0.2f;
        }

        int boneIndex = 1;
        float distance = 0f;
        for (; boneIndex < _snakeSkeleton.ActiveBones.Count; boneIndex++)
        {
            distance = headDistance - boneIndex * boneDistance;
            if (distance < 0)
                break;

            var trackPoint = _bonusFinish.GetPositionByDistance(distance);
            var currentBone = _snakeSkeleton.ActiveBones[boneIndex];
            currentBone.Position = trackPoint;

            var forwardVector = _bonusFinish.GetPositionByDistance(distance + 0.01f) - currentBone.Position;

            _snakeSkeleton.ActiveBones[boneIndex].LookRotation(forwardVector);
        }

        if (headDistance == _bonusFinish.DistanceLength)
            BonusPoleCrawled?.Invoke();
    }

    private void MoveBoneOnTrack(int boneIndex, float boneDistance, float upShift = 0f)
    {
        var trackPoint = _track.GetPositionByDistance(boneDistance) + Vector3.up * upShift;
        var currentBone = _snakeSkeleton.ActiveBones[boneIndex];
        currentBone.Position = trackPoint;

        if (boneDistance + 0.1f > _track.DistanceLength)
            return;

        var forwardVector = _track.GetPositionByDistance(boneDistance + 0.1f) - currentBone.Position + Vector3.up * upShift;
        _snakeSkeleton.ActiveBones[boneIndex].LookRotation(forwardVector);
    }
}
