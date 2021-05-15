using System.Collections;
using UnityEngine;

public class SinMoving : MonoBehaviour
{
    [SerializeField] private Transform _point_1;
    [SerializeField] private Transform _point_2;
    [SerializeField] private Transform _movingPoint;
    [SerializeField] private float _speed;

    [SerializeField] private float _frequency = 20.0f;
    [SerializeField] private float _magnitude = 0.5f;

    private float _totalDistance;

    private void Start()
    {
        _movingPoint.position = _point_1.position;
        _totalDistance = Vector3.Distance(_point_1.position, _point_2.position);

        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        Debug.Log("Start Moving");

        while(_movingPoint.position.z <= 10)
        {
            Vector3 movePosition = Vector3.forward * _speed * Time.deltaTime;
            _movingPoint.position += movePosition;
            float currentDistance = Vector3.Distance(_movingPoint.position, _point_2.position);
            _movingPoint.position += Vector3.right * Mathf.Sin(currentDistance / _totalDistance * _frequency);

            yield return null;
        }

        Debug.Log("Finish Moving");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_point_1.position, _point_2.position);
    }
}
