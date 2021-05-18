#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor;

[CustomEditor(typeof(SnakeDataBase))]
public class SnakeDataBaseEditor : Editor
{
    private SnakeDataBase _dataBase;
    private SerializedProperty _dataBaseList;
    private FieldInfo _dataBaseListInfo;
    private List<string> _boosterDataFields;

    private void OnEnable()
    {
        _dataBase = target as SnakeDataBase;

        _boosterDataFields = new List<string>();
        foreach (var field in typeof(SnakeData).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            _boosterDataFields.Add(field.Name);
        foreach (var field in typeof(GUIDData).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            _boosterDataFields.Add(field.Name);

        FieldInfo[] allFields = _dataBase.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        _dataBaseListInfo = allFields.ToList().Find((field) => field.FieldType.IsEquivalentTo(typeof(List<SnakeData>)));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _dataBaseList = serializedObject.FindProperty(_dataBaseListInfo.Name);

        for (int i = 0; i < _dataBaseList.arraySize; i++)
        {
            SerializedProperty element = _dataBaseList.GetArrayElementAtIndex(i);
            foreach (var item in _boosterDataFields)
                EditorGUILayout.PropertyField(element.FindPropertyRelative(item));

            if (GUILayout.Button(new GUIContent("-", "Удалить")))
                _dataBase.RemoveAt(i);
        }

        if (GUILayout.Button(new GUIContent("+", "Добавить")))
            _dataBase.Add(new SnakeData());

        serializedObject.ApplyModifiedProperties();
    }
}
#endif