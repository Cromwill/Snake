using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : ObstacleOld
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Direction _direction;

    private Transform _selfTransform;

    void Start()
    {
        _selfTransform = GetComponent<Transform>();
    }


    void Update()
    {
        _selfTransform.Rotate(0, _rotationSpeed * Time.deltaTime * (float)_direction, 0);
    }

}

public enum Direction
{
    Left = -1,
    Right = 1
}
