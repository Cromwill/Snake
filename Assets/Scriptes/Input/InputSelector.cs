using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSelector : MonoBehaviour
{
    [SerializeField] private SnakeInitializer _snakeInitializer;
    [SerializeField] private BaseInput _mobileInput;
    [SerializeField] private BaseInput _keyboardInput;

    public event Action<BaseInput> InputSelected;

    private void OnEnable()
    {
        _snakeInitializer.Initialized += OnSnakeInitialized;
    }

    private void OnDisable()
    {
        _snakeInitializer.Initialized -= OnSnakeInitialized;
    }

    private void OnSnakeInitialized(Snake snake)
    {
#if UNITY_EDITOR
        _keyboardInput.Init(snake );
        InputSelected?.Invoke(_keyboardInput);
#else
        _mobileInput.Init(snake);
        InputSelected?.Invoke(_mobileInput);
#endif
    }
}
