using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPole : MonoBehaviour
{
    [SerializeField] private float _inputJumpAngle;

    private Vector3 _inputPosition;
    private float _angleDelta;
    private float _radiusScale;
    private float _distanceLength;

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _inputJumpAngle, 0) * transform.forward * transform.lossyScale.x);
    }

#endif

    public void Init(float angleDelta, float radiusScale, float distanceLength)
    {
        _angleDelta = angleDelta;
        _radiusScale = radiusScale;
        _distanceLength = distanceLength;

        _inputPosition = Quaternion.Euler(0, _inputJumpAngle, 0) * transform.forward;
    }

    public Vector3 GetPositionByParameter(float t)
    {
        float deltaRad = 2 * Mathf.PI * t * _angleDelta;

        var posHeight = transform.position + -transform.up * transform.lossyScale.y + transform.up * 2 * transform.lossyScale.y * t;

        posHeight += transform.forward * Mathf.Cos(deltaRad) * _radiusScale * transform.lossyScale.z / 2f;
        posHeight += transform.right * Mathf.Sin(deltaRad) * _radiusScale * transform.lossyScale.x / 2f;

        return posHeight;
    }

    public IEnumerable<Vector3> GetAllForwardPositions(float startParam = 0f, float endParam = 1f)
    {
        var parameter = 0f;
        var shift = 1f / _angleDelta;

        while (parameter <= 1f)
        {
            if (parameter > startParam && parameter < endParam)
                yield return GetPositionByParameter(parameter);

            parameter += shift;
        }
    }

    public IEnumerable<Vector3> GetAllPosition(float distanceShift, float startParam = 0f, float endParam = 1f)
    {
        var parameter = 0f;
        var shift = distanceShift / _distanceLength;

        while (parameter <= 1f)
        {
            if (parameter > startParam && parameter < endParam)
                yield return GetPositionByParameter(parameter);

            parameter += shift;
        }
    }
}
