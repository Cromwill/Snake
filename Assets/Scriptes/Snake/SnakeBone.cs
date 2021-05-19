using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBone : MonoBehaviour
{
    public Vector3 Position { get => transform.position; set => transform.position = value; }
    public Vector3 LocalPosition { get => transform.localPosition; }

    private Coroutine _enableSmoothlyCoroutine;

    public void EnableSmoothly(float speed)
    {
        if (_enableSmoothlyCoroutine != null)
        {
            StopCoroutine(_enableSmoothlyCoroutine);
            _enableSmoothlyCoroutine = null;
        }

        if (transform.localScale != Vector3.one)
            _enableSmoothlyCoroutine = StartCoroutine(EnableSmoothlyCoroutine(speed));
    }

    public void Enable()
    {
        transform.localScale = Vector3.one;
    }

    public void Disable()
    {
        if (_enableSmoothlyCoroutine != null)
        {
            StopCoroutine(_enableSmoothlyCoroutine);
            _enableSmoothlyCoroutine = null;
        }

        transform.localScale = Vector3.zero;
    }

    private IEnumerator EnableSmoothlyCoroutine(float speed)
    {
        while (transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _enableSmoothlyCoroutine = null;
    }

    public void LookRotation(Vector3 forward)
    {
        var rotation = Quaternion.LookRotation(forward, Vector3.up);
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x - 90f, rotation.eulerAngles.y, rotation.eulerAngles.z);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10f * Time.deltaTime);
        //transform.rotation = rotation;
    }
}
