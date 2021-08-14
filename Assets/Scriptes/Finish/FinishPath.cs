using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPath : MonoBehaviour
{
    [SerializeField] private SimpleLine _line;
    [SerializeField] private Pole _pole;

    public float MaxParameter => 2f;
    public float DistanceLength => _line.DistanceLength + _pole.DistanceLength;

    public Vector3 GetPositionByDistance(float distance)
    {
        var parameter = GetParameterByDistance(distance);

        if (parameter < 1f)
            return _line.GetPositionByParameter(parameter);
        else
            return _pole.GetPositionByParameter(parameter - 1f);
    }

    public float GetParameterByDistance(float distance)
    {
        if (distance <= _line.DistanceLength)
            return distance / _line.DistanceLength;
        else
            return 1 + (distance - _line.DistanceLength) / _pole.DistanceLength;
    }
}
