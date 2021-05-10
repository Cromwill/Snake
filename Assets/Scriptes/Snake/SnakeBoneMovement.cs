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

    public void Move(Track track, float headDistance, float boneDistance)
    {
        _snakeSkeleton.ActiveBones[0].Position = track.GetPosition(headDistance);
        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _snakeSkeleton.ActiveBones[1].Position;
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance * 0.01f;
            if (distance < 0)
                continue;

            _snakeSkeleton.ActiveBones[i].Position = track.GetPosition(distance);
            forwardVector = _snakeSkeleton.ActiveBones[i - 1].Position - _snakeSkeleton.ActiveBones[i].Position;

            _snakeSkeleton.ActiveBones[i].LookRotation(forwardVector);
        }
    }
}
