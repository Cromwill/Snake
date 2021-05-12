using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeBoneScaler : MonoBehaviour
{
    [SerializeField] private float _minDistance = 1.5f;

    private List<Food> _targetFoods;
    private SnakeSkeleton _snakeSkeleton;
    private float _nextScaleRate;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _targetFoods = new List<Food>();
    }

    private void OnEnable()
    {
        _snakeSkeleton.Head.TriggerEntered += OnHeadTriggerEntered;
        _snakeSkeleton.Tail.TriggerExited += OnTailTriggerExited;
    }

    private void OnDisable()
    {
        _snakeSkeleton.Head.TriggerEntered -= OnHeadTriggerEntered;
        _snakeSkeleton.Tail.TriggerExited -= OnTailTriggerExited;
    }

    private void OnHeadTriggerEntered(Collider collider)
    {
        if (collider.TryGetComponent(out Food food))
        {
            _targetFoods.Add(food);
        }
    }

    private void OnTailTriggerExited(Collider collider)
    {
        if (collider.TryGetComponent(out Food food))
        {
            _targetFoods.Remove(food);
            Destroy(food.gameObject);

            for (int i = 0; i < 2; i++)
                _snakeSkeleton.AddBoneInTailSmoothly();

        }
    }

    private void Update()
    {
        if (_targetFoods.Count == 0)
        {
            SetNormalSize();
            return;
        }

        _nextScaleRate = 1f;
        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var scaleDistance = MinDistanceToTarget(_snakeSkeleton.ActiveBones[i].Position);
            //var scaleDistance = Vector3.Distance(_snakeSkeleton.ActiveBones[i].Position, _targetFood.ColliderCenterPosition + Vector3.up * 0.5f);
            if (scaleDistance <= _minDistance)
            {
                ScaleBone(i, Mathf.Sqrt(_minDistance / scaleDistance));
            }
            else
            {
                _snakeSkeleton.ActiveBones[i].transform.localScale = Vector3.one * _nextScaleRate;
                _nextScaleRate = 1f;
            }
        }
    }

    private float MinDistanceToTarget(Vector3 bonePosition)
    {
        float minDistance = float.MaxValue;
        foreach (var food in _targetFoods)
        {
            var distance = Vector3.Distance(bonePosition, food.ColliderCenterPosition + Vector3.up * 0.5f);
            if (distance < minDistance)
                minDistance = distance;
        }

        return minDistance;

    }

    private void SetNormalSize()
    {
        for (int i = 0; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            _snakeSkeleton.ActiveBones[i].EnableSmoothly(1f);
        }
    }

    private void ScaleBone(int boneIndex, float scaleRate)
    {
        _snakeSkeleton.ActiveBones[boneIndex].transform.localScale = Vector3.one * scaleRate * _nextScaleRate;
        _nextScaleRate = 1f / scaleRate;
    }
}
