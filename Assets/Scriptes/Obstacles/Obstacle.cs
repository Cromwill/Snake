using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _obstacleSignals;
    [SerializeField] private Material _workingMaterial;
    [SerializeField] private Material _notWorkingMaterial;

    public void ToggleSignal()
    {
        if (_obstacleSignals != null && _obstacleSignals.Length > 0)
        {
            foreach (var signal in _obstacleSignals)
                signal.material = _notWorkingMaterial;
        }
    }
}
