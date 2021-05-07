using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(SnakeSkeleton))]
public class SnakeSkeletonEditor : Editor
{
    private SnakeSkeleton _snakeSkeleton;
    List<Transform> _bones;

    private void Awake()
    {
        _snakeSkeleton = target as SnakeSkeleton;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Update Armature"))
            UpdateArmatureComponents();

        base.OnInspectorGUI();
    }

    private void UpdateArmatureComponents()
    {
        _bones = new List<Transform>();
        InitNodeList(_snakeSkeleton.Armature.GetChild(0));

        foreach (var bone in _bones)
        {
            if (bone.TryGetComponent(out SnakeBone snakeBone) == false)
                bone.gameObject.AddComponent<SnakeBone>();
        }
    }

    private void InitNodeList(Transform currentNode)
    {
        _bones.Add(currentNode);
        if (currentNode.childCount > 0)
            InitNodeList(currentNode.GetChild(0));
    }
}
