using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class FoodSceneRedactor : MonoBehaviour
{
    [SerializeField] private Spline _track;
    [SerializeField] private Food _template;
    [SerializeField] private Transform _foodContainer;

    public Food Template => _template;
    public Transform FoodContainer => _foodContainer;
    
    public CurveSample GetProjectPosition(Vector3 point)
    {
        return _track.GetProjectionSample(point);
    }
}
