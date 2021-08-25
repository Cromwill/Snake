using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool _isShop;

    private AdSettings _adSettings;

    private void Awake()
    {
        _adSettings = Singleton<AdSettings>.Instance;
        _adSettings.RequestRewarded();
        _adSettings.ShowBanner();

        if (_isShop)
            return;

        var currentLevelData = new CurrentLevelData();
        currentLevelData.Load(new JsonSaveLoad());

        if (SceneManager.GetActiveScene().buildIndex != currentLevelData.CurrentLevel)
            SceneManager.LoadScene(currentLevelData.CurrentLevel);

    }

    private void OnEnable()
    {
        _adSettings.InterstitialShowed += LoadNextLevelAfterAd;
        _adSettings.InterstitialShowTryed += LoadNextLevelAfterAd;
    }

    private void OnDisable()
    {
        _adSettings.InterstitialShowed -= LoadNextLevelAfterAd;
        _adSettings.InterstitialShowTryed -= LoadNextLevelAfterAd;
    }

    private void LoadNextLevelAfterAd()
    {
        var currentLevelData = new CurrentLevelData();
        currentLevelData.Load(new JsonSaveLoad());

        currentLevelData.IncreaseLevel();
        currentLevelData.Save(new JsonSaveLoad());

        SceneManager.LoadScene(currentLevelData.CurrentLevel);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop_v2");
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

    public void LoadlLevelWithSave(int index)
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
        _adSettings.ShowInterstitial();
    }

    public void LoadHatCollection()
    {
        SceneManager.LoadScene("HatCollection");
    }
}
