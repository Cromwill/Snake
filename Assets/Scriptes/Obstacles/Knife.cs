using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float _loweringSpeed;
    [SerializeField] private float _liftingSpeed;
    [SerializeField] private float _delayTime;

    private Transform _selfTransform;
    private Direction _direction;

    private const float MIN_Z_ROTATION = 270;

    private void Start()
    {
        _selfTransform = GetComponent<Transform>();
        _direction = Direction.Left;
    }


    private void Update()
    {
        if (_delayTime <= 0)
        {
            var speed = _direction == Direction.Left ? _loweringSpeed : _liftingSpeed;
            _selfTransform.Rotate(0, 0, speed * (int)_direction * Time.deltaTime, Space.World);
            CheckDirection();
        }
        else
            _delayTime -= Time.deltaTime;
    }

    private void CheckDirection()
    {
        if (_selfTransform.rotation.eulerAngles.z != 0)
        {
            if (_direction == Direction.Left && _selfTransform.rotation.eulerAngles.z < MIN_Z_ROTATION)
                _direction = Direction.Right;
            else if (_direction == Direction.Right && _selfTransform.rotation.eulerAngles.z > 359)
                _direction = Direction.Left;
        }
    }
}
