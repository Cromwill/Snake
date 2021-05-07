using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSkeleton : MonoBehaviour
{
    [SerializeField] private Transform _armature;

    public int MinLength => 3;
    public int CurrentLength { get; private set; }
    public Transform Armature => _armature;
    public List<SnakeBone> ActiveBones => _activeBones;

    private List<SnakeBone> _bones;
    private List<SnakeBone> _activeBones;

    private void Start()
    {
        _bones = new List<SnakeBone>();
        _activeBones = new List<SnakeBone>();
        InitNodeList(_armature.GetChild(0));

        for (int i = 0; i < _bones.Count; i++)
        {
            if (i < MinLength)
            {
                _activeBones.Add(_bones[i]);
                _bones[i].Enable();
            }
            else
                _bones[i].Disable();

            _bones[i].gameObject.AddComponent<SnakeBone>();
        }
    }

    private void InitNodeList(Transform currentNode)
    {
        _bones.Add(currentNode.GetComponent<SnakeBone>());
        if (currentNode.childCount > 0)
            InitNodeList(currentNode.GetChild(0));
    }

    public void AddBoneInTail()
    {
        if (_activeBones.Count >= _bones.Count)
            return;

        var addedBone = _bones[_activeBones.Count];
        addedBone.Enable();
        _activeBones.Add(addedBone);
    }

    public void RemoveBoneFromTail()
    {
        if (_activeBones.Count <= MinLength)
            return;

        var removedBone = _activeBones[_activeBones.Count - 1];
        removedBone.Disable();

        _activeBones.RemoveAt(_activeBones.Count - 1);
    }
}
