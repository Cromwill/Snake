using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool _isShop;

    private void Awake()
    {
        if (_isShop)
            return;

        var currentLevelData = new CurrentLevelData();
        currentLevelData.Load(new JsonSaveLoad());

        if (SceneManager.GetActiveScene().buildIndex != currentLevelData.CurrentLevel)
            SceneManager.LoadScene(currentLevelData.CurrentLevel);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadCurrentLevel()
    {
        var currentLevel = new CurrentLevelData();
        currentLevel.Load(new JsonSaveLoad());

        SceneManager.LoadScene(currentLevel.CurrentLevel);
    }

    public void LoadLevel(int index)
    {
        var currentLevel = new CurrentLevelData();
        currentLevel.Load(new JsonSaveLoad());

        while (currentLevel.CurrentLevel != index)
            currentLevel.IncreaseLevel();

        currentLevel.Save(new JsonSaveLoad());

        SceneManager.LoadScene(index);
    }

    public void LoadNextLevel()
    {
        var currentLevelData = new CurrentLevelData();
        currentLevelData.Load(new JsonSaveLoad());

        currentLevelData.IncreaseLevel();
        currentLevelData.Save(new JsonSaveLoad());

        SceneManager.LoadScene(currentLevelData.CurrentLevel);
    }
}
