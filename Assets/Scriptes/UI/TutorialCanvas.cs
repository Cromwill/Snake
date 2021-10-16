using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private GameCanvas _gameCanvas;

    private void OnEnable()
    {
        _gameCanvas.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameCanvas.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        gameObject.SetActive(false);
    }
}
