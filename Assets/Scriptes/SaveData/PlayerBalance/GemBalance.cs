using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GemBalance : ISavedObject
{
    [SerializeField] private int _balance;

    public int Balance => _balance;

    public static event UnityAction<int> GemChanged;

    public void Add(int value)
    {
        _balance += value;
        GemChanged?.Invoke(Balance);
    }

    public bool SpendGem(int value)
    {
        if (Balance < value)
            return false;

        _balance -= value;

        GemChanged?.Invoke(Balance);
        return true;
    }

    public void Load(ISaveLoadVisiter saveLoadVisiter)
    {
        var saved = saveLoadVisiter.Load(this);

        _balance = saved.Balance;
    }

    public void Save(ISaveLoadVisiter saveLoadVisiter)
    {
        saveLoadVisiter.Save(this);
    }
}
