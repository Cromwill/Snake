using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hat data base", menuName = "Hats/HatDataBase", order = 51)]
public class HatDataBase : ScriptableObject
{
    [SerializeField] private int _defaultHatIndex = 0;
    [SerializeField] private Sprite _hatPlaceholder;
    [SerializeField] private List<HatData> _hats = new List<HatData>();

    public IEnumerable<HatData> Data => _hats;
    public HatData DefaultData => _hats[_defaultHatIndex];
    public Sprite HatPlaceholder => _hatPlaceholder;

    public void Add(HatData data)
    {
        _hats.Add(data);
    }

    public void RemoveAt(int index)
    {
        _hats.RemoveAt(index);
    }

    public void MoveFront(int index)
    {
        if (index < 1 || index > _hats.Count - 1)
            return;

        var temp = _hats[index];
        _hats[index] = _hats[index - 1];
        _hats[index - 1] = temp;
    }

    public void MoveBack(int index)
    {
        if (index >= _hats.Count - 1 || index < 0)
            return;

        var temp = _hats[index];
        _hats[index] = _hats[index + 1];
        _hats[index + 1] = temp;
    }
}
