using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppMetricaEventSender : MonoBehaviour
{
    public enum LevelDiff { Easy, Medium, Hard, }
    public enum LevelType { Normal, Bonus, }

    [SerializeField] private GameCanvas _startTrigger;
    [SerializeField] private FinishTrigger _finishTrigger;
    [SerializeField] private BonusFinish _bonusFinish;
    [SerializeField] private LevelDiff _levelDiff;
    [SerializeField] private LevelType _levelType;

    private const string GameMode = "classic";
    private const string LevelCountKey = nameof(LevelCountKey);

    private DateTime _startLevelTime;
    private bool _isStarted;
    private AdSettings _adSettings;

    private void OnEnable()
    {
        _adSettings = Singleton<AdSettings>.Instance;

        _startTrigger.GameStarted += OnLevelStart;
        if (_finishTrigger)
            _finishTrigger.PlayerFinished += OnLevelFinished;
        if (_bonusFinish)
            _bonusFinish.Finished += OnLevelFinished;
        _adSettings.VideoAdsAviable += OnVideoAdsAviable;
        _adSettings.VideoAdsStarted += OnVideoAdsStarted;
        _adSettings.VideoAdsWatched += OnVideoAdsWatched;
    }

    private void OnDisable()
    {
        _startTrigger.GameStarted -= OnLevelStart;
        if (_finishTrigger)
            _finishTrigger.PlayerFinished -= OnLevelFinished;
        if (_bonusFinish)
            _bonusFinish.Finished -= OnLevelFinished;
        _adSettings.VideoAdsAviable -= OnVideoAdsAviable;
        _adSettings.VideoAdsStarted -= OnVideoAdsStarted;
        _adSettings.VideoAdsWatched -= OnVideoAdsWatched;
    }

    public void ForceInitialize(bool isBonus)
    {
        _startTrigger = FindObjectOfType<GameCanvas>();
        _finishTrigger = FindObjectOfType<FinishTrigger>();
        _bonusFinish = FindObjectOfType<BonusFinish>();

        _levelDiff = LevelDiff.Medium;
        _levelType = isBonus ? LevelType.Bonus : LevelType.Normal;
    }

    private void ShowAdEvent(string eventName, string adType, string placement, string result, bool connection)
    {
        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("ad_type", adType);
        eventParameters.Add("placement", placement);
        eventParameters.Add("result", result);
        eventParameters.Add("connection", connection);

        AppMetrica.Instance.ReportEvent(eventName, eventParameters);
    }
    private void OnVideoAdsAviable(string adType, string placement, string result, bool connection)
    {
        ShowAdEvent("video_ads_aviable", adType, placement, result, connection);
    }

    private void OnVideoAdsStarted(string adType, string placement, string result, bool connection)
    {
        ShowAdEvent("video_ads_started", adType, placement, result, connection);
    }

    private void OnVideoAdsWatched(string adType, string placement, string result, bool connection)
    {
        ShowAdEvent("video_ads_watched", adType, placement, result, connection);
    }

    private void OnLevelStart()
    {
        int levelCount = 1;
        if (PlayerPrefs.HasKey(LevelCountKey))
            levelCount = PlayerPrefs.GetInt(LevelCountKey) + 1;

        PlayerPrefs.SetInt(LevelCountKey, levelCount);

        var levelData = new CurrentLevelData();
        levelData.Load(new JsonSaveLoad());

        var levelNumber = SceneManager.GetActiveScene().buildIndex + 1;
        var levelName = SceneManager.GetActiveScene().name;
        var levelDiff = _levelDiff.ToString();
        var levelLoop = levelData.LevelLoop;
        var levelRandom = 0;
        var levelType = _levelType.ToString();
        var gameMode = GameMode;

        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("level_number", levelNumber);
        eventParameters.Add("level_name", levelName);
        eventParameters.Add("level_count", levelCount);
        eventParameters.Add("level_diff", levelDiff);
        eventParameters.Add("level_loop", levelLoop);
        eventParameters.Add("level_random", levelRandom);
        eventParameters.Add("level_type", levelType);
        eventParameters.Add("game_mode", gameMode);

        AppMetrica.Instance.ReportEvent("level_start", eventParameters);
        AppMetrica.Instance.SendEventsBuffer();

        _startLevelTime = DateTime.Now;
        _isStarted = true;
    }

    private void OnLevelFinished(bool isWin, int progress, string customResult = null)
    {
        var levelData = new CurrentLevelData();
        levelData.Load(new JsonSaveLoad());

        var levelNumber = SceneManager.GetActiveScene().buildIndex + 1;
        var levelName = SceneManager.GetActiveScene().name;
        var levelCount = PlayerPrefs.GetInt(LevelCountKey);
        var levelDiff = _levelDiff.ToString();
        var levelLoop = levelData.LevelLoop;
        var levelRandom = 0;
        var levelType = _levelType.ToString();
        var gameMode = GameMode;
        var result = customResult != null ? customResult : isWin ? "win" : "loose";
        var timeSec = (DateTime.Now - _startLevelTime).Seconds;

        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("level_number", levelNumber);
        eventParameters.Add("level_name", levelName);
        eventParameters.Add("level_count", levelCount);
        eventParameters.Add("level_diff", levelDiff);
        eventParameters.Add("level_loop", levelLoop);
        eventParameters.Add("level_random", levelRandom);
        eventParameters.Add("level_type", levelType);
        eventParameters.Add("game_mode", gameMode);
        eventParameters.Add("result", result);
        eventParameters.Add("time", timeSec);
        eventParameters.Add("progress", progress);
        eventParameters.Add("continue", 1);

        AppMetrica.Instance.ReportEvent("level_finish", eventParameters);
        AppMetrica.Instance.SendEventsBuffer();

        _isStarted = false;
    }

    private void OnLevelFinished()
    {
        OnLevelFinished(true, 1);
    }

    private void OnApplicationQuit()
    {
        if (_isStarted == false)
            return;

        var progress = FindObjectOfType<ProgressView>();

        OnLevelFinished(false, (int)(progress.Value * 100), "leave");

        Debug.Log("OnAppQuit2");
    }
}
