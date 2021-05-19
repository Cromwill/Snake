using UnityEngine;
using UnityEngine.Events;

public class SnakeBoneMovement : MonoBehaviour
{
    [SerializeField] private bool _curveMovement;
    [SerializeField] private float _curveAmplitude;
    [SerializeField] private float _curveSpeed;

    public event UnityAction<float> Partially—rawled;
    public event UnityAction Full—rawled;

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
        _snakeSkeleton.ActiveBones[0].Position = _track.GetPositionByDistance(headDistance);

        if (headDistance + boneDistance > _track.DistanceLength)
            return;

        var forwardVector = -_snakeSkeleton.ActiveBones[0].Position + _track.GetPositionByDistance(headDistance + boneDistance);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance;
            if (distance < 0)
                break;

            MoveBoneOnTrack(i, distance);
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

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _finishPath.GetPositionByDistance(headDistance - 0.01f);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

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

            forwardVector = _snakeSkeleton.ActiveBones[boneIndex - 1].Position - _snakeSkeleton.ActiveBones[boneIndex].Position;

            _snakeSkeleton.ActiveBones[boneIndex].LookRotation(forwardVector);
        }

        if (headDistance == _finishPath.DistanceLength)
            Full—rawled?.Invoke();
        else if (boneIndex == _snakeSkeleton.ActiveBones.Count && _finishPath.GetParameterByDistance(distance) >= 1f)
            Partially—rawled?.Invoke(headDistance);
    }

    private void MoveBoneOnTrack(int boneIndex, float boneDistance)
    {
        var trackPoint = _track.GetPositionByDistance(boneDistance);
        var currentBone = _snakeSkeleton.ActiveBones[boneIndex];
        currentBone.Position = trackPoint;

        //var forwardVector = _snakeSkeleton.ActiveBones[boneIndex - 1].Position - _snakeSkeleton.ActiveBones[boneIndex].Position;
        var forwardVector = _track.GetPositionByDistance(boneDistance + 0.01f) - _snakeSkeleton.ActiveBones[boneIndex].Position;

        _snakeSkeleton.ActiveBones[boneIndex].LookRotation(forwardVector);

        if (_curveMovement)
        {
            int center = _snakeSkeleton.ActiveBones.Count / 2;

            var delta = boneDistance;
            var amplitude = _curveAmplitude * (1 - (Mathf.Abs(boneIndex - center) / (float)_snakeSkeleton.ActiveBones.Count));
            currentBone.Position += currentBone.transform.right * amplitude * Mathf.Sin(delta * _curveSpeed);
        }
    }
}
