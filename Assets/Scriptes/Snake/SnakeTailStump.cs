using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeTailStump : MonoBehaviour
{
    private SnakeSkeleton _snakeSkeleton;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
    }

    public void SetStructure(SnakeSkeleton targetStructure, Track track, float headDistance, float boneDistance)
    {
        _snakeSkeleton.SetInitialTailSize();

        for (int i = 0; i < targetStructure.ActiveBones.Count - _snakeSkeleton.MinLength; i++)
            _snakeSkeleton.AddBoneInTail();

        _snakeSkeleton.ActiveBones[0].Position = track.GetPosition(headDistance);
        _snakeSkeleton.ActiveBones[0].Position += _snakeSkeleton.ActiveBones[0].transform.right * Random.Range(-0.2f, 0.2f);

        if (headDistance < float.Epsilon)
            return;

        var forwardVector = _snakeSkeleton.ActiveBones[0].Position - _snakeSkeleton.ActiveBones[1].Position;
        _snakeSkeleton.ActiveBones[0].LookRotation(forwardVector);

        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var distance = headDistance - i * boneDistance * 0.01f;
            if (distance < 0)
                continue;

            var trackPoint = track.GetPosition(distance);
            var currentBone = _snakeSkeleton.ActiveBones[i];
            currentBone.Position = trackPoint;

            currentBone.Position += currentBone.transform.right * Random.Range(-0.1f, 0.1f);

            forwardVector = _snakeSkeleton.ActiveBones[i - 1].Position - _snakeSkeleton.ActiveBones[i].Position;

            _snakeSkeleton.ActiveBones[i].LookRotation(forwardVector);
        }
    }
}
