using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSelector : MonoBehaviour
{
    [SerializeField] private Snake _moveableObject;
    [SerializeField] private BaseInput _mobileInput;
    [SerializeField] private BaseInput _keyboardInput;

    private void OnValidate()
    {
        if (_moveableObject is IMoveable)
            return;

        _moveableObject = null;
    }

    private void Start()
    {
#if UNITY_EDITOR
        _keyboardInput.Init(_moveableObject as IMoveable);
#else
        _mobileInput.Init(_moveableObject);
#endif
    }
}
