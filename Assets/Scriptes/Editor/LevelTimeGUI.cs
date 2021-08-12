using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelTimeGUI : EditorWindow
{
    private static Track _track;
    private static Obstacle[] _obstacles;
    private static GUIStyle _normalStyle;
    private static GUIStyle _errorStyle;

    [MenuItem("Window/Level Time GUI/Save time #s")]
    public static void SaveTimeToFile()
    {
        if (_track == null)
            _track = FindObjectOfType<Track>();
        _obstacles = FindObjectsOfType<Obstacle>();

        if (_track == null)
        {
            Debug.LogError("Save error: Can't find track");
            return;
        }

        var filePath = Directory.GetCurrentDirectory() + "\\Assets\\LevelsTime.json";
        var levelList = new LevelTimeList();

        if (File.Exists(filePath))
            levelList = LevelTimeList.Load(filePath);

        var completedTime = GetTime();
        var currentSceneName = SceneManager.GetActiveScene().name;
        var levelTime = new CompleteLevelTime(currentSceneName, completedTime);

        levelList.EditLevel(levelTime);
        LevelTimeList.Save(filePath, levelList);

        Debug.Log($"{currentSceneName} level time saved to {filePath}");
    }

    [MenuItem("Window/Level Time GUI/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate += OnScene;
        Debug.Log("Level Time GUI : Enabled");

        _normalStyle = new GUIStyle();
        _normalStyle.fontSize = 18;
        _normalStyle.normal.textColor = Color.blue;

        _errorStyle = new GUIStyle();
        _errorStyle.fontSize = 18;
        _errorStyle.normal.textColor = Color.red;
    }

    [MenuItem("Window/Level Time GUI/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Level Time GUI : Disabled");
    }

    private static void OnScene(SceneView sceneview)
    {
        Handles.BeginGUI();

        if (_track == null)
            _track = FindObjectOfType<Track>();
        _obstacles = FindObjectsOfType<Obstacle>();

        if (_track == null)
        {
            GUILayout.Label("Can't find track object", _errorStyle);
        }
        else
        {
            GUILayout.Label($"Track length: {_track.DistanceLength}", _normalStyle);
            GUILayout.Label($"Obstacles count: {_obstacles.Length}", _normalStyle);

            var time = GetTime();

            GUILayout.Label($"Aproximate time to complete a level: {time} seconds", _normalStyle);
        }

        Handles.EndGUI();
    }

    private static float GetTime()
    {
        var snakeSpeed = 20f;
        var obstacleWaitingTime = 3f;
        var levelCompletedTime = _track.DistanceLength / snakeSpeed + obstacleWaitingTime * _obstacles.Length;

        return levelCompletedTime;
    }
}

[Serializable]
public class LevelTimeList
{
    [SerializeField] private List<CompleteLevelTime> _levels;

    private const string LevelTimeListKey = nameof(LevelTimeListKey);

    public IEnumerable<CompleteLevelTime> Levels => _levels;

    public LevelTimeList()
    {
        _levels = new List<CompleteLevelTime>();
    }

    public void EditLevel(CompleteLevelTime levelTime)
    {
        int findedIndex = -1;
        for (int i = 0; i < _levels.Count; i++)
        {
            if (string.Equals(_levels[i].LevelName, levelTime.LevelName))
            {
                findedIndex = i;
                break;
            }
        }

        if (findedIndex >= 0)
            _levels[findedIndex] = levelTime;
        else
            _levels.Add(levelTime);
    }

    public static LevelTimeList Load(string filePath)
    {
        var jsonValue = File.ReadAllText(filePath);
        return JsonUtility.FromJson<LevelTimeList>(jsonValue);
    }

    public static void Save(string filePath, LevelTimeList list)
    {
        var jsonValue = JsonUtility.ToJson(list, true);
        File.WriteAllText(filePath, jsonValue);
    }
}

[Serializable]
public struct CompleteLevelTime
{
    [SerializeField] private string _levelName;
    [SerializeField] private float _time;

    public string LevelName => _levelName;
    public float Time => _time;

    public CompleteLevelTime(string levelName, float time)
    {
        _levelName = levelName;
        _time = time;
    }
}