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
    private Head _snakeHead;
    private int _maxBones;
    private int _currentSnakeLength;

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
        _snakeHead = snake.GetComponentInChildren<Head>();

        _snakeHead.FoodFinded += OnFoodFinded;
        _skeleton.BoneRemoved += OnBoneRemoved;

        _maxBones = _skeleton.Bones.Count();
        _currentSnakeLength = _skeleton.MinLength;
    }

    private void OnFoodFinded(Food food)
    {
        _currentSnakeLength += 2;
        SetDistance(_currentSnakeLength);
    }

    private void OnBoneRemoved()
    {
        _currentSnakeLength -= 2;
        SetDistance(_currentSnakeLength);
    }

    private void SetDistance(int snakeLength)
    {
        var distance = (snakeLength - _skeleton.MinLength) / (float)_maxBones * 2f;
        distance = Mathf.Clamp(distance, 0, 1f);
        _animator.SetFloat("CameraDistance", distance);
    }
}
