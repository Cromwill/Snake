using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineKnife : Obstacle
{
    [SerializeField] private Transform _knife;
    [SerializeField] private float _liftingSpeed;
    [SerializeField] private float _loweringSpeed;

    private int _direction;

    private const float UPPER_Y_POSITION = 4.0f;
    private const float LOWER_Y_POSITION = 0.5f;

    void Start()
    {
        _knife.localPosition = new Vector3(0, UPPER_Y_POSITION, 0);
        _direction = -1;
    }


    void Update()
    {
        float speed = _direction > 0 ? _liftingSpeed : _loweringSpeed;

        _knife.Translate(Vector3.up * _direction * (speed * Time.deltaTime), Space.Self);

        if (_direction < 0 && _knife.localPosition.y < LOWER_Y_POSITION)
            _direction = 1;
        else if (_direction > 0 && _knife.localPosition.y > UPPER_Y_POSITION)
            _direction = -1;
    }
}
