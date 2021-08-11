using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using SplineMesh;

[CustomEditor(typeof(FoodSceneRedactor))]
public class FoodRedactorEditor : Editor
{
    public enum EditType
    {
        Remove, Add, None,
    }

    private FoodSceneRedactor _foodRedactor;
    private EditType _editType;

    private void Awake()
    {
        _foodRedactor = target as FoodSceneRedactor;
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;
        if (e.isMouse && e.button != 0)
            return;

        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                GUIUtility.hotControl = controlID;
                Raycast();
                e.Use();
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                break;
            case EventType.KeyDown:
                GUIUtility.keyboardControl = controlID;
                if (Event.current.keyCode == KeyCode.LeftShift)
                    _editType = EditType.Add;
                else if (Event.current.keyCode == KeyCode.LeftControl)
                    _editType = EditType.Remove;
                break;
            case EventType.KeyUp:
                GUIUtility.keyboardControl = controlID;
                if (Event.current.keyCode == KeyCode.LeftShift || Event.current.keyCode == KeyCode.LeftShift)
                    _editType = EditType.None;
                break;
        }
    }

    private void Raycast()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (_editType == EditType.Add)
                TryAdd(hitInfo);
            else if (_editType == EditType.Remove)
                TryDelete(hitInfo);
        }
    }

    private void TryAdd(RaycastHit hitInfo)
    {
        var splineSample = _foodRedactor.GetProjectPosition(hitInfo.point);
        var worldPosition = splineSample.location;

        if (_foodRedactor.FoodContainer != null)
            worldPosition = _foodRedactor.FoodContainer.InverseTransformPoint(splineSample.location);

        _foodRedactor.Template.transform.position = worldPosition;
        Debug.Log(splineSample.timeInCurve);
        PrefabUtility.InstantiatePrefab(_foodRedactor.Template, _foodRedactor.FoodContainer);
    }

    private void TryDelete(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent(out Food food))
            DestroyImmediate(food.gameObject);
    }
}
