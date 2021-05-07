using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSelector : MonoBehaviour
{
    [SerializeField] private SplineSnake _moveableObject;
    [SerializeField] private BaseInput _mobileInput;
    [SerializeField] private BaseInput _keyboardInput;

    private void Start()
    {
#if UNITY_EDITOR
        _keyboardInput.Init(_moveableObject as IMoveable);
#else
        _mobileInput.Init(_moveableObject);
#endif
    }
}
