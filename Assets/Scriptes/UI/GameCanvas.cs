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

    private void Awake()
    {
        _selfCanvas = GetComponent<Canvas>();
        _pole = FindObjectOfType<Pole>();
        _snakeInitializer = FindObjectOfType<SnakeInitializer>();
    }

    private void OnEnable()
    {
        _pole.SnakeCrawled += OnSnakeCrawled;
        _snakeInitializer.Initialized += OnSnakeInitialized;
    }

    private void OnDisable()
    {
        _pole.SnakeCrawled -= OnSnakeCrawled;

        if (_snake)
            _snake.StartMoving -= OnSnakeMoving;
    }

    private void OnSnakeCrawled(int gemValue)
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
