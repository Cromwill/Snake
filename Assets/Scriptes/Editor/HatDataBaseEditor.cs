#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor;
using System.Text;

[CustomEditor(typeof(HatDataBase))]
public class HatDataBaseEditor : Editor
{
    private HatDataBase _dataBase;
    private SerializedProperty _dataBaseList;
    private FieldInfo _dataBaseListInfo;
    private int _currentRenderIndex;

    private void OnEnable()
    {
        _dataBase = target as HatDataBase;

        FieldInfo[] allFields = _dataBase.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        _dataBaseListInfo = allFields.ToList().Find((field) => field.FieldType.IsEquivalentTo(typeof(List<HatData>)));

        _currentRenderIndex = 0;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _dataBaseList = serializedObject.FindProperty(_dataBaseListInfo.Name);

        GUIStyle headerTextStyle = new GUIStyle();
        headerTextStyle.fontSize = 35;
        headerTextStyle.normal.textColor = Color.green;

        GUIStyle defaultTextStyle = new GUIStyle();
        defaultTextStyle.fontSize = 18;
        defaultTextStyle.normal.textColor = Color.white;

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("HAT DATA BASE", headerTextStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(50);

        var failedIndexes = GetFailedIndexes(_dataBaseList);
        if (failedIndexes.Count > 0)
        {
            StringBuilder value = new StringBuilder();
            foreach (var index in failedIndexes)
                value.Append(index + ", ");

            EditorGUILayout.HelpBox("Не вся база заполнена!\nПроверьте следующие элементы:\n" + value, MessageType.Warning);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_defaultHatIndex"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_hatPlaceholder"));
        GUILayout.Space(20);

        EditorGUILayout.LabelField("Hat count: " + _dataBaseList.arraySize, defaultTextStyle);
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current hat index: " + _currentRenderIndex, defaultTextStyle);

        if (GUILayout.Button(new GUIContent("<", "Предыдущий элемент"), GUILayout.Width(30), GUILayout.Height(30)))
            _currentRenderIndex = (_currentRenderIndex > 0) ? _currentRenderIndex - 1 : _currentRenderIndex;
        if (GUILayout.Button(new GUIContent(">", "Следующий элемент"), GUILayout.Width(30), GUILayout.Height(30)))
            _currentRenderIndex = (_currentRenderIndex < _dataBaseList.arraySize - 1) ? _currentRenderIndex + 1 : _currentRenderIndex;
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("+", "Добавить"), GUILayout.Width(30), GUILayout.Height(30)))
            _dataBase.Add(new HatData());
        if (GUILayout.Button(new GUIContent("-", "Удалить"), GUILayout.Width(30), GUILayout.Height(30)))
            _dataBase.RemoveAt(_currentRenderIndex);

        if (_currentRenderIndex >= _dataBase.Data.Count())
            _currentRenderIndex = _dataBase.Data.Count() - 1;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("<<", "Сместить влево"), GUILayout.Width(30), GUILayout.Height(30)))
            _dataBase.MoveFront(_currentRenderIndex);
        if (GUILayout.Button(new GUIContent(">>", "Сместить вправо"), GUILayout.Width(30), GUILayout.Height(30)))
            _dataBase.MoveBack(_currentRenderIndex);

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        var element = _dataBaseList.GetArrayElementAtIndex(_currentRenderIndex);
        RenderElement(element);
        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void RenderElement(SerializedProperty element)
    {
        EditorGUILayout.PropertyField(element.FindPropertyRelative("_hatTemplate"));
        EditorGUILayout.PropertyField(element.FindPropertyRelative("_hatPreview"));

        var sprite = element.FindPropertyRelative("_hatPreview").objectReferenceValue as Sprite;

        if (sprite)
            GUILayout.Label(sprite.texture, GUILayout.Width(150), GUILayout.Height(150));
    }

    private List<int> GetFailedIndexes(SerializedProperty array)
    {
        var failedList = new List<int>();

        for (int i = 0; i < array.arraySize; i++)
        {
            var element = _dataBaseList.GetArrayElementAtIndex(i);

            var hatTemplate = element.FindPropertyRelative("_hatTemplate");
            var hatPreview = element.FindPropertyRelative("_hatPreview");

            if (hatTemplate.objectReferenceValue == null || hatPreview.objectReferenceValue == null)
                failedList.Add(i);
        }

        return failedList;
    }
}
#endif