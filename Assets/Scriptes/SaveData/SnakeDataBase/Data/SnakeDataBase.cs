using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Snake data base", menuName = "Shop/Snakes/SnakeDataBase", order = 51)]
public class SnakeDataBase : ScriptableObject
{
    [SerializeField] private int _defaultSnakeIndex = 0;
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
    public void MoveFront(int index)
    {
        if (index < 1 || index > _snakes.Count - 1)
            return;

        var temp = _snakes[index];
        _snakes[index] = _snakes[index - 1];
        _snakes[index - 1] = temp;
    }

    public void MoveBack(int index)
    {
        if (index >= _snakes.Count - 1 || index < 0)
            return;

        var temp = _snakes[index];
        _snakes[index] = _snakes[index + 1];
        _snakes[index + 1] = temp;
    }
}
