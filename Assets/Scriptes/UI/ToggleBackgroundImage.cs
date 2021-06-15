using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBackgroundImage : MonoBehaviour
{
    [SerializeField] private ToggleWater _toggleWater;
    [SerializeField] private GameObject _backroundPink;
    [SerializeField] private GameObject _backgroundSky;

    private bool _pinkRender;

    private void Start()
    {
        HideAll();
        _pinkRender = true;
    }

    public void ToogleBackgroundImage()
    {
        _toggleWater.HideAll();
        HideAll();

        if (_pinkRender)
            _backroundPink.SetActive(true);
        else
            _backgroundSky.SetActive(true);

        _pinkRender = !_pinkRender;
    }

    public void HideAll()
    {
        _backroundPink.SetActive(false);
        _backgroundSky.SetActive(false);
    }
}
