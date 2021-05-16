using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] private float _angleDelta;

    public float DistanceLength { get; private set; }

    private void OnValidate()
    {
        if (_angleDelta <= 0)
            _angleDelta = 1f;
    }

    private void Start()
    {
        DistanceLength = transform.lossyScale.y * _angleDelta;
    }

    public Vector3 GetPositionByParameter(float t)
    {
        float deltaRad = 2 * Mathf.PI * t * _angleDelta;

        var posHeight = transform.position + Vector3.down * transform.lossyScale.y + Vector3.up * 2 * transform.lossyScale.y * t;

        posHeight += transform.forward * Mathf.Cos(deltaRad) * transform.lossyScale.z / 2f;
        posHeight += transform.right * Mathf.Sin(deltaRad) * transform.lossyScale.x / 2f;

        return posHeight;
    }
}
