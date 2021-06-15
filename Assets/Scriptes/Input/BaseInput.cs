using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInput : MonoBehaviour
{
    protected abstract event UnityAction Touched;
    protected abstract event UnityAction Released;

    private IMoveable _moveableObject;

    private void OnEnable()
    {
        Touched += OnTouched;
        Released += OnReleased;
    }

    public void Init(IMoveable moveableObject) => _moveableObject = moveableObject;

    private void OnTouched()
    {
        _moveableObject?.StartMove();
    }

    private void OnReleased()
    {
        _moveableObject?.EndMove();
    }

    private void OnDisable()
    {
        Touched -= OnTouched;
        Released -= OnReleased;
    }
}
