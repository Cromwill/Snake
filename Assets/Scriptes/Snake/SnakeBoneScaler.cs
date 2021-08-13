using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeBoneScaler : MonoBehaviour
{
    [SerializeField] private float _minDistance = 1.5f;

    private List<Food> _targetFoods;
    private SnakeSkeleton _snakeSkeleton;
    private FinishTrigger _finishTrigger;
    private float _nextScaleRate;

    private void Awake()
    {
        _finishTrigger = FindObjectOfType<FinishTrigger>();
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _targetFoods = new List<Food>();
    }

    private void OnEnable()
    {
        _snakeSkeleton.Head.TriggerEntered += OnHeadTriggerEntered;
        _snakeSkeleton.Tail.TriggerExited += OnTailTriggerExited;

        if (_finishTrigger)
            _finishTrigger.PlayerFinished += OnPlayerFinished;
    }

    private void OnDisable()
    {
        _snakeSkeleton.Head.TriggerEntered -= OnHeadTriggerEntered;
        _snakeSkeleton.Tail.TriggerExited -= OnTailTriggerExited;

        if (_finishTrigger)
            _finishTrigger.PlayerFinished -= OnPlayerFinished;
    }

    private void OnHeadTriggerEntered(Collider collider)
    {
        if (collider.TryGetComponent(out Food food))
        {
            food.Eating();
            _targetFoods.Add(food);
        }
    }

    private void OnTailTriggerExited(Collider collider)
    {
        if (collider.TryGetComponent(out Food food))
            EatFood(food);
    }

    private void OnPlayerFinished()
    {
        while (_targetFoods.Count != 0)
            EatFood(_targetFoods[0]);
    }

    private void EatFood(Food food)
    {
        if (_targetFoods.Contains(food) == false)
            return;

        _targetFoods.Remove(food);
        Destroy(food.gameObject);

        _snakeSkeleton.AddBoneInTailSmoothly();
        _snakeSkeleton.AddBoneInTailSmoothly();
    }

    private void FixedUpdate()
    {
        if (_targetFoods.Count == 0)
        {
            SetNormalSize();
            return;
        }

        _nextScaleRate = 1f;
        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            //if (_snakeSkeleton.ActiveBones[i].Enabled == false)
            //    continue;
            var scaleDistance = MinDistanceToTarget(_snakeSkeleton.ActiveBones[i].Position);
            if (scaleDistance < _minDistance)
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
            var distance = Vector3.Distance(bonePosition, food.ColliderCenterPosition + Vector3.up * 0.3f);
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
