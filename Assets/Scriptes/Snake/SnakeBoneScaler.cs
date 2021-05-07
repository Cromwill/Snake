using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeBoneScaler : MonoBehaviour
{
    [SerializeField] private Transform _armature;
    [SerializeField] private float _minDistance = 1.5f;

    private Transform _targetFood;
    private SnakeSkeleton _snakeSkeleton;
    private float _nextScaleRate;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
    }

    private void OnEnable()
    {
        _snakeSkeleton.Head.TriggerEntered += OnHeadTriggerEntered;
        _snakeSkeleton.Tail.TriggerExited += OnTailTriggerExited;
    }

    private void OnHeadTriggerEntered(Collider collider)
    {
        if (collider.TryGetComponent(out Food food))
        {
            _targetFood = food.transform;
        }
    }

    private void OnTailTriggerExited(Collider collider)
    {
        if (collider.TryGetComponent(out Food food))
        {
            _targetFood = null;
            Destroy(food.gameObject);
            _snakeSkeleton.AddBoneInTail();
        }
    }

    public void Init(Transform targetFood)
    {
        _targetFood = targetFood;
    }

    private void Update()
    {
        if (_targetFood == null)
            return;

        _nextScaleRate = 1f;
        for (int i = 0; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var scaleDistance = Vector3.Distance(_snakeSkeleton.ActiveBones[i].Position, _targetFood.position);
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
