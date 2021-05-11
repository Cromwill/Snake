using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer _obstacleSignal;
    [SerializeField] private Material _workingMaterial;
    [SerializeField] private Material _notWorkingMaterial;

    public void ToggleSignal()
    {
        if (_obstacleSignal != null)
            _obstacleSignal.material = _notWorkingMaterial;
    }
}
