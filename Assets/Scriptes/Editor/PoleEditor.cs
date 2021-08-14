using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

[CustomEditor(typeof(Pole))]
public class PoleEditor : Editor
{
    private Pole _pole;
    private float _finishSnakeLength;
    private float _poleDistance;

    private void Awake()
    {
        _pole = target as Pole;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Check"))
            CalculateParameters();

        var infoString = new StringBuilder();
        infoString.Append("Finish Snake Length: " + _finishSnakeLength + "\n");
        infoString.Append("Pole Distance: " + _poleDistance + "\n");

        if (_poleDistance <= _finishSnakeLength)
            EditorGUILayout.HelpBox(infoString.ToString() + "The snake will be able to crawl to the top", MessageType.Info);
        else
            EditorGUILayout.HelpBox(infoString.ToString() + "The snake will NOT be able to crawl to the top", MessageType.Warning);

        base.OnInspectorGUI();
    }

    private void CalculateParameters()
    {
        var startSnakeLength = 4;
        var distanceBetweenSegments = 1.5f;
        var foodCount = FindObjectsOfType<Food>().Length;
        var incrementPerFood = 2;

        _finishSnakeLength = (startSnakeLength + foodCount * incrementPerFood) * distanceBetweenSegments;
        _poleDistance = _pole.DistanceLength;
    }
}
