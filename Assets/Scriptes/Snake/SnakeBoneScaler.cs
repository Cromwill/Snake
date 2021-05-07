using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeBoneScaler : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Transform _armature;
    [SerializeField] private float _minDistance = 1.5f;

    private SnakeSkeleton _snakeSkeleton;
    private float _nextScaleRate;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
    }

    private void Update()
    {
        if (_targetPoint == null)
            return;

        _nextScaleRate = 1f;
        for (int i = 0; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var scaleDistance = Vector3.Distance(_snakeSkeleton.ActiveBones[i].Position, _targetPoint.position);
            if (scaleDistance <= _minDistance)
            {
                ScaleBone(i, _minDistance / scaleDistance);
            }
            else
            {
                _snakeSkeleton.ActiveBones[i].transform.localScale = Vector3.one * _nextScaleRate;
                _nextScaleRate = 1f;
            }
        }
    }

    private void ScaleBone(int boneIndex, float scaleRate)
    {
        _snakeSkeleton.ActiveBones[boneIndex].transform.localScale = Vector3.one * scaleRate * _nextScaleRate;
        _nextScaleRate = 1f / scaleRate;
    }
}
