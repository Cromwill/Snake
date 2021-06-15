using UnityEngine;

public class ToggleWater : MonoBehaviour
{
    [SerializeField] private ToggleBackgroundImage _toggleBackgroundImage;
    [SerializeField] private GameObject _toonWater;
    [SerializeField] private GameObject _realWater;

    private bool _toonRender;

    private void Start()
    {
        _toonRender = true;

        ToogleWater();
    }

    public void ToogleWater()
    {
        _toggleBackgroundImage.HideAll();
        HideAll();

        if (_toonRender)
            _toonWater.SetActive(true);
        else
            _realWater.SetActive(true);

        _toonRender = !_toonRender;
    }

    public void HideAll()
    {
        _toonWater.SetActive(false);
        _realWater.SetActive(false);
    }
}
