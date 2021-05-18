using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CurrentLevelData : ISavedObject
{
    private const int LevelCount = 2;

    [SerializeField] private int _currentLevel;

    public int CurrentLevel => _currentLevel;

    public CurrentLevelData()
    {
        _currentLevel = 0;
    }

    public void IncreaseLevel()
    {
        _currentLevel = (_currentLevel + 1) % LevelCount;
    }

    public void Load(ISaveLoadVisiter saveLoadVisiter)
    {
        var save = saveLoadVisiter.Load(this);

        _currentLevel = save._currentLevel;
    }

    public void Save(ISaveLoadVisiter saveLoadVisiter)
    {
        saveLoadVisiter.Save(this);
    }
}
