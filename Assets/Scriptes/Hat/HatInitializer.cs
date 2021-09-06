using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatInitializer : MonoBehaviour
{
    [SerializeField] private HatDataBase _hatDataBase;
    [SerializeField] private SnakeInitializer _snakeInitializer;

    private Snake _instSnake;

    private void OnEnable()
    {
        _snakeInitializer.Initialized += OnSnakeInitizlized;
    }

    private void OnDisable()
    {
        _snakeInitializer.Initialized -= OnSnakeInitizlized;
    }

    private void OnSnakeInitizlized(Snake snake)
    {
        _instSnake = snake;

        HatCollection hatCollection = new HatCollection(_hatDataBase);
        hatCollection.Load(new JsonSaveLoad());

        var selectedHat = hatCollection.SelectedHat;
        if (selectedHat == null)
            return;

        var snakeHead = _instSnake.GetComponentInChildren<Head>();
        var inst = Instantiate(selectedHat.Prefab, snakeHead.transform.position, Quaternion.identity);
        inst.PutOn(snakeHead, false);
    }

    public void ForceInitialize()
    {
        _snakeInitializer = FindObjectOfType<SnakeInitializer>();
    }
}
