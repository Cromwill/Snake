using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class FoodSceneRedactor : MonoBehaviour
{
    [SerializeField] private Spline _track;
    [SerializeField] private Food _template;
    [SerializeField] private Transform _foodContainer;
    [SerializeField] private float _minSpawnDistance = 2f;
    [SerializeField] private float _angle = 0f;

    private void OnValidate()
    {
        if (_foodContainer != null && _foodContainer.TryGetComponent(out MonoBehaviour behaviour))
            _foodContainer = null;
    }

    public Food Template => _template;
    public Transform FoodContainer => _foodContainer;
    public float MinSpawnDistance => _minSpawnDistance;
    public float Angle => _angle;
    
    public CurveSample GetProjectPosition(Vector3 point)
    {
        return _track.GetProjectionSample(point);
    }
}
