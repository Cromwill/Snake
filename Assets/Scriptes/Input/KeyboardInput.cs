using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardInput : BaseInput
{
    protected override event UnityAction Touched;
    protected override event UnityAction Released;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Touched?.Invoke();
        else if (Input.GetKeyUp(KeyCode.Space))
            Released?.Invoke();
    }
}
