using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLine : MonoBehaviour
{
    [SerializeField] private Track _track;
    [SerializeField] private Pole _pole;
    [SerializeField] private Transform _startLinePoint;
    [SerializeField] private Transform _endLinePoint;

    public float DistanceLength { get; private set; }

    private void Start()
    {
        _startLinePoint.position = _track.GetPositionByDistance(_track.DistanceLength);
        _endLinePoint.position = _pole.GetPositionByParameter(0);

        DistanceLength = Vector3.Distance(_endLinePoint.position, _startLinePoint.position);
    }

    public Vector3 GetPositionByParameter(float t)
    {
        var direction = _endLinePoint.position - _startLinePoint.position;
        direction.Normalize();

        var lineLength = Vector3.Distance(_endLinePoint.position, _startLinePoint.position);

        return _startLinePoint.position + direction * lineLength * t;
    }

    public Vector3 GetPositionByDistance(float distance)
    {
        return GetPositionByParameter(distance / DistanceLength);
    }
}
