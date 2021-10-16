using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class HatSpawnerEditorWindow : EditorWindow
{
    private string _levelPattern = @"\d{3}_Normal";
    private int _parameter = 5;

    [MenuItem("Window/Hat Spawner")]
    public static void OpenWindow()
    {
        var window = GetWindow<HatSpawnerEditorWindow>("HatSpawnerEditorWindow");
        window.minSize = new Vector2(500, 200);
        window.maxSize = new Vector2(500, 200);
        //window.ShowModalUtility();
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Проходит по всем уровням, имя которых удовлетворяет паттерну, и изменяет настройки копмонента HatSpawner\n" +
            "Parameter - на уровнях, кратных этому числу, будет спавниться новая шляпка", MessageType.Info);

        _parameter = EditorGUILayout.IntField("Parameter", _parameter);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Update"))
            UpdateSpawners();
    }

    private void UpdateSpawners()
    {
        var currentScene = EditorSceneManager.GetActiveScene().path;

        int index = 0;
        var scenes = EditorBuildSettings.scenes;
        foreach (var scene in scenes)
        {
            EditorSceneManager.OpenScene(scene.path);

            var sceneName = EditorSceneManager.GetActiveScene().name;
            if (Regex.IsMatch(sceneName, _levelPattern) == false)
                continue;

            var spawner = FindObjectOfType<HatSpawner>();
            spawner.Init(index % _parameter == 0);
            Debug.Log(EditorSceneManager.GetActiveScene().name + " " + (index % _parameter == 0));
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            index++;
        }

        EditorSceneManager.OpenScene(currentScene);
    }
}
