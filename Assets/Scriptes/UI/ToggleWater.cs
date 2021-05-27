using UnityEngine;

public class ToggleWater : MonoBehaviour
{
    [SerializeField] private GameObject _toonWater;
    [SerializeField] private GameObject _realWater;

    private void Start()
    {
        if (_toonWater.activeSelf && _realWater.activeSelf)
            _realWater.SetActive(false);
        if (!_toonWater.activeSelf && !_realWater.activeSelf)
            _toonWater.SetActive(true);
    }

    public void ToogleWater()
    {
        _toonWater.SetActive(!_toonWater.activeSelf);
        _realWater.SetActive(!_realWater.activeSelf);
    }
}
