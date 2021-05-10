using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float _loweringSpeed;
    [SerializeField] private float _liftingSpeed;
    [SerializeField] private float _delayTime;
    [SerializeField] private Direction _direction;

    private Transform _selfTransform;

    private const float MIN_Z_ROTATION = 270;

    private void Start()
    {
        _selfTransform = GetComponent<Transform>();
    }


    private void Update()
    {
        //if (_delayTime <= 0)
        //{
        //    var speed = _direction == Direction.Left ? _loweringSpeed : _liftingSpeed;
        //    _selfTransform.Rotate(0, 0, speed * (int)_direction * Time.deltaTime, Space.World);
        //    CheckDirection();
        //}
        //else
        //    _delayTime -= Time.deltaTime;
    }

    private void CheckDirection()
    {
        if (_selfTransform.rotation.eulerAngles.z != 0)
        {
            if (_direction == Direction.Left && _selfTransform.rotation.eulerAngles.z < MIN_Z_ROTATION)
            {
                _direction = Direction.Right;
                Vector3 rotation = _selfTransform.rotation.eulerAngles;
                _selfTransform.rotation = Quaternion.Euler(rotation.x, rotation.y, MIN_Z_ROTATION);
            }
            else if (_direction == Direction.Right && _selfTransform.rotation.eulerAngles.z > 0 &&
                _selfTransform.rotation.eulerAngles.z < 270)
            {
                _direction = Direction.Left;
                Vector3 rotation = _selfTransform.rotation.eulerAngles;
                _selfTransform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            }
        }
    }
}
