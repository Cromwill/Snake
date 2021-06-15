using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SnakeInventory : ISavedObject
{
    [SerializeField] private List<string> _buyedGUID = new List<string>();
    [SerializeField] private string _selectedGUID;

    private SnakeDataBase _dataBase;

    public IEnumerable<SnakeData> Data => from data in _dataBase.Data
                                            where _buyedGUID.Contains(data.GUID)
                                            select data;
    public SnakeData SelectedSnake => _dataBase.Data.First((data) => data.GUID == _selectedGUID);

    public SnakeInventory(SnakeDataBase dataBase)
    {
        _dataBase = dataBase;
        if (string.IsNullOrEmpty(_selectedGUID))
        {
            Add(_dataBase.DefaultData);
            SelectSnake(_dataBase.DefaultData);
        }
    }

    public void Add(SnakeData data)
    {
        _buyedGUID.Add(data.GUID);
    }

    public bool Remove(SnakeData data)
    {
        return _buyedGUID.Remove(data.GUID);
    }

    public bool Contains(SnakeData data)
    {
        return _buyedGUID.Contains(data.GUID);
    }

    public void SelectSnake(SnakeData data)
    {
        _selectedGUID = data.GUID;
    }

    public void Load(ISaveLoadVisiter saveLoadVisiter)
    {
        var saved = saveLoadVisiter.Load(this);

        _buyedGUID = saved._buyedGUID;
        _selectedGUID = saved._selectedGUID;

        if (string.IsNullOrEmpty(_selectedGUID))
            SelectSnake(_dataBase.DefaultData);
    }

    public void Save(ISaveLoadVisiter saveLoadVisiter)
    {
        saveLoadVisiter.Save(this);
    }
}
