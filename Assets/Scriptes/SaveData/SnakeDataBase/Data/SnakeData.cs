using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class SnakeData : GUIDData
{
    [SerializeField] private Sprite _preview;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private Snake _prefab;

    public Sprite Preview => _preview;
    public string Name => _name;
    public string Description => _description;
    public int Price => _price;
    public Snake Prefab => _prefab;
}
