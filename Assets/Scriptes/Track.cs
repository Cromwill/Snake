using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SplineMesh;

public class Track : MonoBehaviour
{
    [SerializeField] private Spline _spline;

    public float DistanceLength => _spline.Length;

    public Vector3 GetPositionByDistance(float length)
    {
        CurveSample sample = _spline.GetSampleAtDistance(length);

        return sample.location;
    }
}