using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour, IMoveable
{
    private float _speed;

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void EndMove()
    {
        _speed = 0f;
    }

    public void StartMove()
    {
        _speed = 5f;
    }
}
