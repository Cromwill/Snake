using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

[RequireComponent(typeof(Animator))]
public class CameraDistance : MonoBehaviour
{
    [SerializeField] private SnakeInitializer _snakeInitializer;

    private Animator _animator;
    private SnakeSkeleton _skeleton;
    private int _maxBones;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _snakeInitializer.Initialized += OnSnakeInitizliaed;
    }

    private void OnDisable()
    {
        _snakeInitializer.Initialized -= OnSnakeInitizliaed;
    }

    private void OnSnakeInitizliaed(Snake snake)
    {
        _skeleton = snake.GetComponent<SnakeSkeleton>();

        _maxBones = _skeleton.Bones.Count();
    }

    private void Update()
    {
        var distance = (_skeleton.ActiveBones.Count - _skeleton.MinLength) / (float)_maxBones * 2f;
        distance = Mathf.Clamp(distance, 0, 1f);
        _animator.SetFloat("CameraDistance", distance);
    }
}
