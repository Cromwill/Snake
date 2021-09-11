using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RateUsInitializer : MonoBehaviour
{
    [SerializeField] private RateUsWindow _rateUsWindow;

    private const string RateUsCantShowKey = nameof(RateUsCantShowKey);
    private const int FirstSceneNumShow = 4;
    private const int DelaySecond = 259200;

    private SerializableDateTime _lastShowTime;
    private RateUsWindow _instWindow;

    private bool CantShow => PlayerPrefs.HasKey(RateUsCantShowKey);

    private void OnEnable()
    {
        _lastShowTime = new SerializableDateTime();
        _lastShowTime.Load(new JsonSaveLoad());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
     
        _lastShowTime.Save(new JsonSaveLoad());
    }

    private void Awake()
    {
        var others = FindObjectOfType<RateUsInitializer>();
        if (this.Equals(others) == false)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            PlayerPrefs.DeleteKey(RateUsCantShowKey);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (CantShow)
            return;

        var dateDiff = DateTime.Now.Subtract(_lastShowTime.ToDateTime);
        var delayed = dateDiff.TotalSeconds > DelaySecond;

        if (delayed || (scene.buildIndex + 1 == FirstSceneNumShow))
            ShowRateUsWindow();
    }

    private void ShowRateUsWindow()
    {
        _instWindow = Instantiate(_rateUsWindow);

        _instWindow.Closed += OnRateUsWindowClosed;
        _instWindow.Successfully += OnRateUsSuccessfull;
    }

    private void OnRateUsWindowClosed()
    {
        _lastShowTime = new SerializableDateTime(DateTime.Now);

        _instWindow.Closed -= OnRateUsWindowClosed;
        _instWindow.Successfully -= OnRateUsSuccessfull;
    }

    private void OnRateUsSuccessfull()
    {
        PlayerPrefs.SetInt(RateUsCantShowKey, 1);

        _instWindow.Closed -= OnRateUsWindowClosed;
        _instWindow.Successfully -= OnRateUsSuccessfull;
    }
}

[Serializable]
public struct SerializableDateTime : ISavedObject
{
    [SerializeField] private int _year;
    [SerializeField] private int _month;
    [SerializeField] private int _day;
    [SerializeField] private int _hour;
    [SerializeField] private int _minute;
    [SerializeField] private int _second;

    public DateTime ToDateTime => new DateTime(_year, _month, _day, _hour, _minute, _second);

    public SerializableDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        _year = year;
        _month = month;
        _day = day;
        _hour = hour;
        _minute = minute;
        _second = second;
    }

    public SerializableDateTime(DateTime dateTime)
        : this(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second)
    { }

    public void Save(ISaveLoadVisiter saveLoadVisiter)
    {
        saveLoadVisiter.Save(this);
    }

    public void Load(ISaveLoadVisiter saveLoadVisiter)
    {
        var saved = saveLoadVisiter.Load(this);
        _year = saved._year;
        _month = saved._month;
        _day = saved._day;
        _hour = saved._hour;
        _minute = saved._minute;
        _second = saved._second;
    }
}
