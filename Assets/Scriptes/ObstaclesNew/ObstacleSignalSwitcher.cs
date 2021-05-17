using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSignalSwitcher : MonoBehaviour
{
    [SerializeField] private Renderer _signalRenderer;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _greenMaterial;

    public void SetRed()
    {
        _signalRenderer.material = _redMaterial;
    }

    public void SetGreen()
    {
        _signalRenderer.material = _greenMaterial;
    }
}
