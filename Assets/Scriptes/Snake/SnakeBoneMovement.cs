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

    public void Init(SnakeSkeleton snakeSkeleton)
    {
        _snakeSkeleton = snakeSkeleton;
    }

    public void Move(Track track, int trackIndex, float headDistance, float boneDistance)
    {
        _snakeSkeleton.ActiveBones[0].Position = track.GetPositionByIndex(headDistance, trackIndex);

        if (headDistance < float.Epsilon)
            return;

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - track.GetPositionByIndex(headDistance - 0.001f, trackIndex);
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance * 0.01f;
            if (distance < 0)
                continue;

            var trackPoint = track.GetPositionByIndex(distance, trackIndex);
            var currentBone = _snakeSkeleton.ActiveBones[i];
            currentBone.Position = trackPoint;

            Debug.Log("Bones - " + _snakeSkeleton.ActiveBones.Count);

            if (_curveMovement)
            {
                int center = _snakeSkeleton.ActiveBones.Count / 2;
                var amplitudeIndex = center  - Mathf.Abs(i - center);
                var delta = distance * track.DistanceLength;
                var amplitude = _curveAmplitude * amplitudeIndex;
                currentBone.Position += currentBone.transform.right * amplitude * Mathf.Sin(delta * _curveSpeed);
            }

            forwardVector = _snakeSkeleton.ActiveBones[i - 1].Position - _snakeSkeleton.ActiveBones[i].Position;

            _snakeSkeleton.ActiveBones[i].LookRotation(forwardVector);
        }
    }
}
