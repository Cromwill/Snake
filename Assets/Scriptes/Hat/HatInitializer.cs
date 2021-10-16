using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatInitializer : MonoBehaviour
{
    [SerializeField] private HatDataBase _hatDataBase;
    [SerializeField] private SnakeInitializer _snakeInitializer;

    private const string HatInitializerSpawnKey = nameof(HatInitializerSpawnKey);
    private Snake _instSnake;

    public bool SpawnEnabled => PlayerPrefs.HasKey(HatInitializerSpawnKey);

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

        if (SpawnEnabled == false)
            return;

        HatCollection hatCollection = new HatCollection(_hatDataBase);
        hatCollection.Load(new JsonSaveLoad());

        var selectedHat = hatCollection.SelectedHat;
        if (selectedHat == null)
            return;

        var snakeHead = _instSnake.GetComponentInChildren<Head>();
        var inst = Instantiate(selectedHat.Prefab, _instSnake.transform.position, Quaternion.identity);
        inst.PutOn(snakeHead, false);
    }

    public void ForceInitialize()
    {
        _snakeInitializer = FindObjectOfType<SnakeInitializer>();
    }

    public static void DisableSpawn()
    {
        PlayerPrefs.DeleteKey(HatInitializerSpawnKey);
    }

    public static void EnableSpawn()
    {
        PlayerPrefs.SetInt(HatInitializerSpawnKey, 1);
    }
}
