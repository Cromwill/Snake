using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CurrentLevelData : ISavedObject
{
    private const int LevelCount = 54;

    [SerializeField] private int _currentLevel;
    [SerializeField] private int _levelLoop;

    public int CurrentLevel => _currentLevel;
    public int LevelLoop => _levelLoop;

    public CurrentLevelData()
    {
        _currentLevel = 0;
        _levelLoop = 0;
    }

    public void IncreaseLevel()
    {
        _currentLevel = (_currentLevel + 1) % LevelCount;

        if (_currentLevel == 0)
            _levelLoop++;
    }

    public void Load(ISaveLoadVisiter saveLoadVisiter)
    {
        var save = saveLoadVisiter.Load(this);

        _currentLevel = save._currentLevel;
        _levelLoop = save._levelLoop;
    }

    public void Save(ISaveLoadVisiter saveLoadVisiter)
    {
        saveLoadVisiter.Save(this);
    }
}
