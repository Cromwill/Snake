using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using SplineMesh;
using System.Text;

[CustomEditor(typeof(FoodSceneRedactor))]
public class FoodRedactorEditor : Editor
{
    public enum EditType
    {
        Remove, Add, None,
    }

    private FoodSceneRedactor _foodRedactor;
    private EditType _editType;
    private Vector3 _startDragPosition;

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
                Raycast(true);
                e.Use();
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                break;
            case EventType.MouseUp:
                GUIUtility.hotControl = controlID;
                _startDragPosition = Vector3.one * float.MaxValue;
                e.Use();
                break;
            case EventType.MouseDrag:
                GUIUtility.hotControl = controlID;
                Raycast(false);
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

    private void Raycast(bool startDrag = false)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (_editType == EditType.Add)
                TryAdd(hitInfo);
            else if (_editType == EditType.Remove)
                TryDelete(hitInfo);

            var splineSample = _foodRedactor.GetProjectPosition(hitInfo.point);
            if (startDrag)
                _startDragPosition = splineSample.location;
        }
    }

    private void TryAdd(RaycastHit hitInfo)
    {
        var splineSample = _foodRedactor.GetProjectPosition(hitInfo.point);
        var worldPosition = splineSample.location;

        if (Vector3.Distance(worldPosition, _startDragPosition) < _foodRedactor.MinSpawnDistance)
            return;

        _startDragPosition = worldPosition;

        if (_foodRedactor.FoodContainer != null)
            worldPosition = _foodRedactor.FoodContainer.InverseTransformPoint(splineSample.location);

        _foodRedactor.Template.transform.position = worldPosition;
        PrefabUtility.InstantiatePrefab(_foodRedactor.Template, _foodRedactor.FoodContainer);
    }

    private void TryDelete(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent(out Food food))
            DestroyImmediate(food.gameObject);
    }

    public override void OnInspectorGUI()
    {
        var infoText = new StringBuilder();
        infoText.Append("shift + ËÊÌ = add food\n");
        infoText.Append("ctrl + ËÊÌ = remove food\n");
        infoText.Append("Mouse dragging supported");
        EditorGUILayout.HelpBox(infoText.ToString(), MessageType.Info, true);

        base.OnInspectorGUI();
    }
}
