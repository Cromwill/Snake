using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : Obstacle
{
    [SerializeField] private float _time;
    [SerializeField] private float _delayStart;
    [SerializeField] private AnimationCurve _curve;

    private Transform _selfTransform;
    private Direction _direction;

    private const float MAX_X_ROTATION = 70.0f;
    private const float MIN_X_ROTATION = -70.0f;



    void Start()
    {
        _selfTransform = GetComponent<Transform>();
        _selfTransform.localRotation = Quaternion.Euler(MAX_X_ROTATION, _selfTransform.localRotation.y, _selfTransform.localRotation.z);
        _direction = Direction.Left;
    }

    void Update()
    {

    }

    private void CheckDirection()
    {

    }
}
