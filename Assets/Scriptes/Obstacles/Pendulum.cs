using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : Obstacle
{
    [SerializeField] private float _speed;
    [SerializeField] private float _delayStart;

    private Transform _selfTransform;
    private Direction _direction;

    private const float MAX_X_ROTATION = 70.0f;
    private const float MIN_X_ROTATION = 290.0f;



    void Start()
    {
        _selfTransform = GetComponent<Transform>();
        _selfTransform.rotation = Quaternion.Euler(MAX_X_ROTATION, _selfTransform.rotation.eulerAngles.y, _selfTransform.rotation.eulerAngles.z);
        _direction = Direction.Left;
    }

    void Update()
    {
        if (_delayStart <= 0)
        {
            _selfTransform.Rotate(_speed * (int)_direction * Time.deltaTime, 0, 0);
            CheckDirection();
        }
        else
            _delayStart -= Time.deltaTime;
    }

    private void CheckDirection()
    {
        if(_direction == Direction.Left)
        {
            if (_selfTransform.rotation.eulerAngles.x > 180 && _selfTransform.rotation.eulerAngles.x < MIN_X_ROTATION)
                _direction = Direction.Right;
        }
        else if(_direction == Direction.Right)
        {
            if (_selfTransform.rotation.eulerAngles.x < 180 && _selfTransform.rotation.eulerAngles.x > MAX_X_ROTATION)
                _direction = Direction.Left;
        }
    }
}
