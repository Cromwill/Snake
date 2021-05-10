using System.Collections;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Direction _direction;
    [SerializeField] private float _delayTime;
    [SerializeField] private float _maxY;
    [SerializeField] private float _minY;

    private Transform _selfTransform;

    private void Start()
    {
        _selfTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (_delayTime <= 0)
        {
            Vector3 scale = _selfTransform.localScale;
            scale.y += speed * (int)_direction * Time.deltaTime;
            _selfTransform.localScale = scale;
            CheckDirection();
        }
        else
            _delayTime -= Time.deltaTime;
    }

    private void CheckDirection()
    {
        if (_direction == Direction.Right && _selfTransform.localScale.y > _maxY)
            _direction = Direction.Left;
        else if (_direction == Direction.Left && _selfTransform.localScale.y < _minY)
            _direction = Direction.Right;
    }
}