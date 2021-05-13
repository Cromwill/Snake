﻿using System.Collections;
using UnityEngine;

public class SnakePunchReaction : MonoBehaviour
{
    [SerializeField] private ParticleSystem _punchEffect;

    private Animator _selfAnimator;
    private Snake _snake;
    private SnakeSkeleton _snakeSkeleton;
    private Coroutine _punchCoroutine;

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
        if (_punchCoroutine != null)
        {
            StopCoroutine(_punchCoroutine);
            _punchCoroutine = StartCoroutine(PunchCoroutine());
            return;
        }

        if (obstacle.IsPuncher)
        {
            _selfAnimator.Play("ObstacleDetected");
            _punchEffect.Play();
            obstacle.OnPlayerPunch();
            _punchCoroutine = StartCoroutine(PunchCoroutine());
        }
    }

    private IEnumerator PunchCoroutine()
    {
        _snake.SetSpeedRate(0.0f);
        yield return new WaitForSeconds(1.5f);

        _snake.SetSpeedRate(1f);
        _punchCoroutine = null;
        _punchEffect.Stop();
    }
}