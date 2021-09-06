using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class HatInitializerEditorWindow : EditorWindow
{
    private string _levelPattern = @"\d{3}_Normal";
    private HatInitializer _template;
    private bool _replaceAll = true;

    [MenuItem("Window/Hat Initizlizer")]
    public static void OpenWindow()
    {
        var window = GetWindow<HatInitializerEditorWindow>("HatInitializerEditorWindow");
        window.minSize = new Vector2(500, 200);
        window.maxSize = new Vector2(500, 200);
        window.ShowModalUtility();
    }

    [Obsolete]
    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Проходит по всем уровням, имя которых удовлетворяет паттерну, и спавнит префаб HatInitializator\n" +
            "Replace All - если true, удаляет предыдущие версии HatInitializer и спавнит его заного", MessageType.Info);

        _levelPattern = EditorGUILayout.TextField("Level pattern", _levelPattern);
        _template = EditorGUILayout.ObjectField("Template", _template, typeof(HatInitializer)) as HatInitializer;
        _replaceAll = EditorGUILayout.Toggle("Replace All", _replaceAll);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Spawn template"))
            SpawnTemplateOnScenes();
    }

    private void SpawnTemplateOnScenes()
    {
        if (_template == null)
        {
            Debug.LogError("Set the HatInitializer template");
            return;
        }

        var currentScene = EditorSceneManager.GetActiveScene().path;

        var scenes = EditorBuildSettings.scenes;
        foreach (var scene in scenes)
        {
            EditorSceneManager.OpenScene(scene.path);

            var sceneName = EditorSceneManager.GetActiveScene().name;
            if (Regex.IsMatch(sceneName, _levelPattern) == false)
                continue;

            var oldHatInitializer = FindObjectOfType<HatInitializer>();
            if (oldHatInitializer)
            {
                if (_replaceAll == false)
                    continue;

                DestroyImmediate(oldHatInitializer.gameObject);
            }

            var inst = PrefabUtility.InstantiatePrefab(_template);
            (inst as HatInitializer).ForceInitialize();

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        EditorSceneManager.OpenScene(currentScene);
    }
}
