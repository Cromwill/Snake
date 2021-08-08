using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HatCollection : ISavedObject
{
    [SerializeField] private List<string> _buyedGUID = new List<string>();
    [SerializeField] private string _selectedGUID;

    private HatDataBase _dataBase;

    public IEnumerable<HatData> Data => from data in _dataBase.Data
                                          where _buyedGUID.Contains(data.GUID)
                                          select data;
    public HatData SelectedHat => _dataBase.Data.First((data) => data.GUID == _selectedGUID);

    public HatCollection(HatDataBase dataBase)
    {
        _dataBase = dataBase;
        if (string.IsNullOrEmpty(_selectedGUID))
        {
            Add(_dataBase.DefaultData);
            SelectHat(_dataBase.DefaultData);
        }
    }

    public void Add(HatData data)
    {
        _buyedGUID.Add(data.GUID);
    }

    public bool Remove(HatData data)
    {
        return _buyedGUID.Remove(data.GUID);
    }

    public bool Contains(HatData data)
    {
        return _buyedGUID.Contains(data.GUID);
    }

    public void SelectHat(HatData data)
    {
        _selectedGUID = data.GUID;
    }

    public void Load(ISaveLoadVisiter saveLoadVisiter)
    {
        var saved = saveLoadVisiter.Load(this);

        _buyedGUID = saved._buyedGUID;
        _selectedGUID = saved._selectedGUID;

        if (string.IsNullOrEmpty(_selectedGUID))
            SelectHat(_dataBase.DefaultData);
    }

    public void Save(ISaveLoadVisiter saveLoadVisiter)
    {
        saveLoadVisiter.Save(this);
    }
}
