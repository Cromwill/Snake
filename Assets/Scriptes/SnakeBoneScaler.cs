using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class SnakeBoneScaler : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Transform _armature;
    [SerializeField] private float _minDistance = 1.5f;

    private List<Transform> _bones;

    private void Start()
    {
        _bones = new List<Transform>();
        InitNodeList(_armature.GetChild(0));
    }

    private void InitNodeList(Transform currentNode)
    {
        _bones.Add(currentNode);
        if (currentNode.childCount > 0)
            InitNodeList(currentNode.GetChild(0));
    }

    private void Update()
    {
        if (_targetPoint == null)
            return;

        for (int i = 0; i < _bones.Count; i++)
        {
            var scaleDistance = Vector3.Distance(_bones[i].position, _targetPoint.position);
            if (scaleDistance <= _minDistance)
            {

            }
        }
    }

    private void ScaleBone(int boneIndex, float scaleRate)
    {
        _bones[boneIndex].localScale *= scaleRate;

        if (boneIndex < _bones.Count - 1)
            _bones[boneIndex - 1].localScale = Vector3.one / scaleRate;
    }
}
