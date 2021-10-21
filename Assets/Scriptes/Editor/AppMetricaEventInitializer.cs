using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class AppMetricaEventInitializer : EditorWindow
{
    private string _levelPattern = @"\d{3}_Normal";
    private AppMetricaEventSender _template;
    private bool _replaceAll = true;

    [MenuItem("Window/AppMetrica Initizlizer")]
    public static void OpenWindow()
    {
        var window = GetWindow<AppMetricaEventInitializer>("AppMetrica");
        window.minSize = new Vector2(500, 200);
        window.maxSize = new Vector2(500, 200);
        //window.ShowModalUtility();
    }

    [Obsolete]
    private void OnGUI()
    {
        EditorGUILayout.HelpBox("???????? ?? ???? ???????, ??? ??????? ????????????? ????????, ? ??????? ?????? HatInitializator\n" +
            "Replace All - ???? true, ??????? ?????????? ?????? HatInitializer ? ??????? ??? ??????", MessageType.Info);

        _levelPattern = EditorGUILayout.TextField("Level pattern", _levelPattern);
        _template = EditorGUILayout.ObjectField("Template", _template, typeof(AppMetricaEventSender)) as AppMetricaEventSender;
        _replaceAll = EditorGUILayout.Toggle("Replace All", _replaceAll);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Spawn template"))
            SpawnTemplateOnScenes();
    }

    private void SpawnTemplateOnScenes()
    {
        if (_template == null)
        {
            Debug.LogError("Set the AppMetricaSender template");
            return;
        }

        var currentScene = EditorSceneManager.GetActiveScene().path;

        var scenes = EditorBuildSettings.scenes;
        foreach (var scene in scenes)
        {
            EditorSceneManager.OpenScene(scene.path);

            bool isBonus = false;
            var sceneName = EditorSceneManager.GetActiveScene().name;
            if (Regex.IsMatch(sceneName, _levelPattern) == false)
                isBonus = true;

             var oldAppMetricaSender = FindObjectOfType<AppMetricaEventSender>();
            if (oldAppMetricaSender)
            {
                if (_replaceAll == false)
                    continue;

                DestroyImmediate(oldAppMetricaSender.gameObject);
            }

            var inst = PrefabUtility.InstantiatePrefab(_template);
            (inst as AppMetricaEventSender).ForceInitialize(isBonus);

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        EditorSceneManager.OpenScene(currentScene);
    }
}
