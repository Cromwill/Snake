using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeAnimations : MonoBehaviour
{
    public static class AnimationParameters
    {
        public static readonly string ObstacleDetected = nameof(ObstacleDetected);
        public static readonly string Eating = nameof(Eating);
    }

    [SerializeField] private Animator _animator;
    [SerializeField] private SnakeBoneMovement _snakeBoneMovement;

    private SnakeSkeleton _snakeSkeleton;

    private void Awake()
    {
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
    }

    private void OnEnable()
    {
        _snakeSkeleton.Head.ObstacleEntered += OnObstacleEntered;
        _snakeSkeleton.Head.FoodFinded += OnFoodFinded;
    }

    private void OnDisable()
    {
        _snakeSkeleton.Head.ObstacleEntered -= OnObstacleEntered;
        _snakeSkeleton.Head.FoodFinded -= OnFoodFinded;
    }

    private void OnFoodFinded(Food food)
    {
        _animator.SetTrigger(AnimationParameters.Eating);
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        _animator.SetTrigger(AnimationParameters.ObstacleDetected);
    }
}
