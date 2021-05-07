using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeSkeleton : MonoBehaviour
{
    [SerializeField] private Transform _armature;
    [SerializeField] private Head _head;
    [SerializeField] private Tail _tail;

    public int MinLength => 3;
    public int CurrentLength { get; private set; }
    public Transform Armature => _armature;
    public List<SnakeBone> ActiveBones => _activeBones;
    public Head Head => _head;
    public Tail Tail => _tail;

    private List<SnakeBone> _bones;
    private List<SnakeBone> _activeBones;

    private void OnEnable()
    {
        _head.ObstacleEntered += OnObstacleEntered;
    }

    private void OnDisable()
    {
        _head.ObstacleEntered -= OnObstacleEntered;
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        Debug.Log("Obstacle: " + obstacle.name);
        RemoveBoneFromTail();
    }

    private void Start()
    {
        _bones = new List<SnakeBone>();
        _activeBones = new List<SnakeBone>();
        InitNodeList(_armature.GetChild(0));

        for (int i = 0; i < _bones.Count; i++)
        {
            _bones[i].Disable();
            if (i < MinLength)
                AddBoneInTail();
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

        _tail.transform.SetParent(addedBone.transform);
        _tail.transform.localPosition = Vector3.zero;
    }

    public void RemoveBoneFromTail()
    {
        if (_activeBones.Count <= MinLength)
            return;

        var removedBone = _activeBones[_activeBones.Count - 1];
        removedBone.Disable();

        _activeBones.RemoveAt(_activeBones.Count - 1);

        _tail.transform.SetParent(_activeBones[_activeBones.Count - 1].transform);
        _tail.transform.localPosition = Vector3.zero;
    }
}
