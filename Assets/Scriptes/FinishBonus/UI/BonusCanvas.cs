using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class BonusCanvas : MonoBehaviour
{
    [SerializeField] private SnakeInitializer _snakeInitializer;
    [SerializeField] private BonusFinish _bonusFinish;

    private Canvas _selfCanvas;
    private Snake _snake;

    private void Awake()
    {
        _selfCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        _snakeInitializer.Initialized += OnSnakeInitialized;
        _bonusFinish.Crawled += OnBonusFinishCrawled;
    }

    private void OnDisable()
    {
        _snakeInitializer.Initialized += OnSnakeInitialized;
        _bonusFinish.Crawled -= OnBonusFinishCrawled;

        if (_snake)
            _snake.StartMoving -= OnSnakeStartMoving;
    }

    private void OnSnakeInitialized(Snake snake)
    {
        _snake = snake;
        _snake.StartMoving += OnSnakeStartMoving;
    }

    private void OnSnakeStartMoving()
    {
        _selfCanvas.enabled = true;
    }

    private void OnBonusFinishCrawled()
    {
        _selfCanvas.enabled = false;
    }
}
