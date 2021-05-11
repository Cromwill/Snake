using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SnakeBoneMovement : MonoBehaviour
{
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

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _snakeSkeleton.ActiveBones[1].Position;
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance * 0.01f;
            if (distance < 0)
                continue;

            var trackPoint = track.GetPositionByIndex(distance, trackIndex);
             _snakeSkeleton.ActiveBones[i].Position = trackPoint;

            forwardVector = _snakeSkeleton.ActiveBones[i - 1].Position - _snakeSkeleton.ActiveBones[i].Position;

            _snakeSkeleton.ActiveBones[i].LookRotation(forwardVector);
        }
    }
}
