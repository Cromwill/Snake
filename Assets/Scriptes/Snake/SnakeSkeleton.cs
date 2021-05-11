using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeSkeleton : MonoBehaviour
{
    [SerializeField] private Transform _armature;
    [SerializeField] private Head _head;
    [SerializeField] private Tail _tail;
    [SerializeField] private float _movingTime;

    public int MinLength => 3;
    public int CurrentLength { get; private set; }
    public Transform Armature => _armature;
    public IEnumerable<SnakeBone> Bones => _bones;
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

    private void Awake()
    {
        _bones = new List<SnakeBone>();
        _activeBones = new List<SnakeBone>();
        InitNodeList(_armature.GetChild(0));

        _head.transform.SetParent(_bones[0].transform);
        _head.transform.localPosition = Vector3.zero;

        for (int i = 0; i < MinLength; i++)
            AddBoneInTail();

        for (int i = _bones.Count - 1; i >= MinLength; i--)
            _bones[i].Disable();

    }

    private void InitNodeList(Transform currentNode)
    {
        _bones.Add(currentNode.GetComponent<SnakeBone>());
        if (currentNode.childCount > 0)
            InitNodeList(currentNode.GetChild(0));
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        RemoveBoneFromTail();
    }

    public void AddBoneInTail()
    {
        if (_activeBones.Count >= _bones.Count)
            return;

        for(int i = 0; i < 2; i++)
        {
            var addedBone = _bones[_activeBones.Count];
            addedBone.Enable();
            _activeBones.Add(addedBone);
        }

        _tail.transform.SetParent(_activeBones[_activeBones.Count - 1].transform);
        _tail.transform.localPosition = Vector3.zero;
    }

    public void RemoveBoneFromTail()
    {
        if (_activeBones.Count <= MinLength)
            return;

        for (int i = 0; i < 2; i++)
        {
            var removedBone = _activeBones[_activeBones.Count - 1];
            removedBone.Disable();

            _activeBones.RemoveAt(_activeBones.Count - 1);
        }

        _tail.transform.SetParent(_activeBones[_activeBones.Count - 1].transform);
        _tail.transform.localPosition = Vector3.zero;
    }
}
