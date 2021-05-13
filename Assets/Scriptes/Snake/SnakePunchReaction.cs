using System.Collections;
using UnityEngine;

public class SnakePunchReaction : MonoBehaviour
{
    [SerializeField] private ParticleSystem _punchEffect;

    private Animator _selfAnimator;
    private Snake _snake;
    private SnakeSkeleton _snakeSkeleton;

    private void Awake()
    {
        _snake = GetComponent<Snake>();
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _selfAnimator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        _snakeSkeleton.Head.ObstacleEntered += OnObstacleEntered;
    }

    private void OnDisable()
    {
        _snakeSkeleton.Head.ObstacleEntered -= OnObstacleEntered;
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        if(obstacle.IsPuncher)
        {
            _selfAnimator.Play("ObstacleDetected");
            _punchEffect.Play();
        }
    }
}