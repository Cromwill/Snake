using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _punchEffect;
    [SerializeField] private MeshRenderer[] _obstacleSignals;
    [SerializeField] private bool _isPuncher;
    [SerializeField] private Material _workingMaterial;
    [SerializeField] private Material _notWorkingMaterial;
    [SerializeField] private Obstacle _parentObstacle;

    public bool IsPuncher => _isPuncher;

    public void ToggleSignal()
    {
        if (_obstacleSignals != null && _obstacleSignals.Length > 0)
        {
            foreach (var signal in _obstacleSignals)
                signal.material = _notWorkingMaterial;
        }
    }

    public virtual void OnPlayerPunch()
    {
        if (_parentObstacle != null)
        {
            _parentObstacle.OnPlayerPunch();
            if (_punchEffect != null)
                Instantiate(_punchEffect, transform.position, Quaternion.identity);
        }
    }
}
