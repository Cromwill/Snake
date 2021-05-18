using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class GameCanvas : MonoBehaviour
{
    [SerializeField] private Pole _pole;

    private Canvas _selfCanvas;

    private void Awake()
    {
        _selfCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        _pole.SnakeCrawled += OnSnakeCrawled;
    }

    private void OnDisable()
    {
        _pole.SnakeCrawled -= OnSnakeCrawled;
    }

    private void OnSnakeCrawled(int gemValue)
    {
        _selfCanvas.enabled = false;
    }
}
