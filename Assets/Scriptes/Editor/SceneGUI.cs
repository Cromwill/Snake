using UnityEditor;
using UnityEngine;

public class SceneGUI : EditorWindow
{
    private static Track _track;
    private static Obstacle[] _obstacles;
    private static GUIStyle _normalStyle;
    private static GUIStyle _errorStyle;

    [MenuItem("Window/Scene GUI/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate += OnScene;
        Debug.Log("Scene GUI : Enabled");

        _normalStyle = new GUIStyle();
        _normalStyle.fontSize = 18;
        _normalStyle.normal.textColor = Color.blue;

        _errorStyle = new GUIStyle();
        _errorStyle.fontSize = 18;
        _errorStyle.normal.textColor = Color.red;
    }

    [MenuItem("Window/Scene GUI/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Scene GUI : Disabled");
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

            var snakeSpeed = 20f;
            var obstacleWaitingTime = 3f;
            var levelCompletedTime = _track.DistanceLength / snakeSpeed + obstacleWaitingTime * _obstacles.Length;

            GUILayout.Label($"Aproximate time to complete a level: {levelCompletedTime} seconds", _normalStyle);
        }

        Handles.EndGUI();
    }
}