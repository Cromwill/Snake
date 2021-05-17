using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Snake data base", menuName = "Shop/Snakes/SnakeDataBase", order = 51)]
public class SnakeDataBase : ScriptableObject
{
    [SerializeField] private int _defaultSnakeIndex;
    [SerializeField] private List<SnakeData> _snakes = new List<SnakeData>();

    public IEnumerable<SnakeData> Data => _snakes;
    public SnakeData DefaultData => _snakes[_defaultSnakeIndex];

    public void Add(SnakeData data)
    {
        _snakes.Add(data);
    }

    public void RemoveAt(int index)
    {
        _snakes.RemoveAt(index);
    }
}
