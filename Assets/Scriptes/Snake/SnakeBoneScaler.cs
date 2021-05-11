using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeBoneScaler : MonoBehaviour
{
    [SerializeField] private float _minDistance = 1.5f;

    private Transform _targetFood;
    private SnakeSkeleton _snakeSkeleton;
    private float _nextScaleRate;
    private Vector3 _normalScale = new Vector3(1.2f, 1.2f, 1.2f);

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
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

    private void Update()
    {
        if (_targetFood == null)
        {
            SetNormalSize();
            return;
        }

        _nextScaleRate = 1f;
        for (int i = 1; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            var scaleDistance = Vector3.Distance(_snakeSkeleton.ActiveBones[i].Position, _targetFood.position + Vector3.up * 0.5f);
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

    private void SetNormalSize()
    {
        for (int i = 0; i < _snakeSkeleton.ActiveBones.Count; i++)
        {
            ScaleBone(i, 1.2f);
        }
    }

    private void ScaleBone(int boneIndex, float scaleRate)
    {
        _snakeSkeleton.ActiveBones[boneIndex].transform.localScale = Vector3.one * scaleRate * _nextScaleRate;
        _nextScaleRate = 1f / scaleRate;
    }
}
