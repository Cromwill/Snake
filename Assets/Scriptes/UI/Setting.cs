using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject[] _notSettingObjects;
    [SerializeField] private GameObject[] _settingObjects;

    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _vibrationToggle;

    private SettingData _data;

    public bool SoundEnable => _data.IsSoundEnable;
    public bool VibrationEnable => _data.IsVibrationEnable;

    private void OnEnable()
    {
        _data = new SettingData(true, true);
        _data.Load(new JsonSaveLoad());
    }

    public void ShowSetting()
    {
        OpenPanels();
        _soundToggle.isOn = _data.IsSoundEnable;
        _vibrationToggle.isOn = _data.IsVibrationEnable;
    }

    public void CloseSetting()
    {
        _data.Save(new JsonSaveLoad());
        ClosePanels();
    }

    public void EnableSound(bool isEnable)
    {
        Debug.Log("Toggle use - " + isEnable);
        _data = new SettingData(isEnable, _data.IsVibrationEnable);
    }

    public void EnableVibratin(bool isEnable)
    {
        Debug.Log("Toggle use - " + isEnable);
        _data = new SettingData(_data.IsSoundEnable, isEnable);
    }

    private void OpenPanels()
    {
        GameObjectSetActive(false, _notSettingObjects);
        GameObjectSetActive(true, _settingObjects);
    }

    private void ClosePanels()
    {
        GameObjectSetActive(true, _notSettingObjects);
        GameObjectSetActive(false, _settingObjects);
    }

    private void GameObjectSetActive(bool isActive, params GameObject[] objects)
    {
        foreach(var obj in objects)
        {
            obj.SetActive(isActive);
        }
    }
}

[Serializable]
public class SettingData : ISavedObject
{
    [SerializeField] private bool _isSoundEnable;
    [SerializeField] private bool _isVibrationEnable;

    public SettingData(bool isSoundEnable, bool isVibrationEnable)
    {
        _isSoundEnable = isSoundEnable;
        _isVibrationEnable = isVibrationEnable;
    }

    public bool IsSoundEnable => _isSoundEnable;
    public bool IsVibrationEnable => _isVibrationEnable;

    public void Load(ISaveLoadVisiter saveLoadVisiter)
    {
        var setting = saveLoadVisiter.Load(this);
        _isSoundEnable = setting._isSoundEnable;
        _isVibrationEnable = setting._isVibrationEnable;
    }

    public void Save(ISaveLoadVisiter saveLoadVisiter)
    {
        saveLoadVisiter.Save(this);
    }
}