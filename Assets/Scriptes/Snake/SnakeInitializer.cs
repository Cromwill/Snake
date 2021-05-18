using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SnakeInitializer : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _snakeDataBase;
    [SerializeField] private Track _track;
    [SerializeField] private FinishPath _finish;
    [SerializeField] private GameObject _tapToPlay;

    public event UnityAction<Snake> Initialized;

    private void Start()
    {
        SnakeInventory snakeInventory = new SnakeInventory(_snakeDataBase);
        snakeInventory.Load(new JsonSaveLoad());

        Debug.Log(snakeInventory.SelectedSnake.Name);

        var selectedSnake = snakeInventory.SelectedSnake.Prefab;

        var inst = Instantiate(selectedSnake, transform.position, transform.rotation);
        inst.Init(_track, _finish, _tapToPlay);

        Initialized?.Invoke(inst);
    }
}
