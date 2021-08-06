using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class GameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _startGroup;

    private Canvas _selfCanvas;
    private Pole _pole;
    private SnakeInitializer _snakeInitializer;
    private Snake _snake;
    private BonusFinish _bonusFinish;

    private void Awake()
    {
        _selfCanvas = GetComponent<Canvas>();
        _pole = FindObjectOfType<Pole>();
        _snakeInitializer = FindObjectOfType<SnakeInitializer>();
        _bonusFinish = FindObjectOfType<BonusFinish>();
    }

    private void OnEnable()
    {
        if (_pole)
            _pole.SnakeCrawled += OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Crawled += OnBonusPoleCrawled;

        _snakeInitializer.Initialized += OnSnakeInitialized;
    }

    private void OnDisable()
    {
        if (_pole)
            _pole.SnakeCrawled -= OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Crawled -= OnBonusPoleCrawled;

        if (_snake)
            _snake.StartMoving -= OnSnakeMoving;

        _snakeInitializer.Initialized -= OnSnakeInitialized;
    }

    private void OnSnakeCrawled(int gemValue)
    {
        _selfCanvas.enabled = false;
    }

    private void OnBonusPoleCrawled()
    {
        _selfCanvas.enabled = false;
    }

    private void OnSnakeInitialized(Snake snake)
    {
        _snake = snake;

        _snake.StartMoving += OnSnakeMoving;
    }

    private void OnSnakeMoving()
    {
        if (_startGroup.activeSelf)
            _startGroup.SetActive(false);
    }
}
