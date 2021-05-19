using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Crosspoint : MonoBehaviour
{
    [SerializeField] private float _upRate;

    private SnakeSkeleton _skeleton;
    private bool _snakeEntered = false;

    private void Start()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Head head))
        {
            if (_snakeEntered == false)
            {
                _snakeEntered = true;
                return;
            }

            if (_skeleton == null)
            {
                _skeleton = head.GetComponentInParent<SnakeSkeleton>();
                enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tail tail))
        {
            _snakeEntered = false;
            _skeleton = null;
            enabled = false;
        }
    }

    private void Update()
    {
        if (_skeleton == null)
            return;

        bool check = false;
        for (int i = 0; i < _skeleton.ActiveBones.Count; i++)
        {
            var bone = _skeleton.ActiveBones[i];
            var distanceToBone = Vector3.Distance(transform.position, bone.Position);
            if (distanceToBone > transform.localScale.x)
            {
                if (check)
                    return;
                else
                    continue;
            }

            var nearRate = 1f - distanceToBone / transform.localScale.x;
            var newPostion = Vector3.up * nearRate * _upRate;
            bone.Position += newPostion;

            check = true;

            if (i + 1 < _skeleton.ActiveBones.Count)
                _skeleton.ActiveBones[i + 1].Position -= newPostion;
        }
    }
}
