using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeSkeleton : MonoBehaviour
{
    [SerializeField] private Transform _armature;
    [SerializeField] private Head _head;
    [SerializeField] private Tail _tail;

    private List<SnakeBone> _bones;
    private List<SnakeBone> _activeBones;

    public int MinLength => 4;
    public int CurrentLength { get; private set; }
    public Transform Armature => _armature;
    public IEnumerable<SnakeBone> Bones => _bones;
    public List<SnakeBone> ActiveBones => _activeBones;
    public Head Head => _head;
    public Tail Tail => _tail;

    public event UnityAction BoneRemoved;

    private void Awake()
    {
        if (_bones == null || _bones.Count == 0)
        {
            _bones = new List<SnakeBone>();
            InitNodeList(_armature.GetChild(0));
        }

        _activeBones = new List<SnakeBone>();

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

    public void AddBoneInTailSmoothly()
    {
        if (_activeBones.Count >= _bones.Count)
            return;

        var addedBone = _bones[_activeBones.Count];
        _activeBones.Add(addedBone);
        addedBone.EnableSmoothly(1f);

        _tail.transform.parent = _activeBones[_activeBones.Count - 1].transform;

        if (_activeBones.Count > 0)
        {
            _bones[_activeBones.Count].transform.localScale = new Vector3(0, 0, 0.01f);
        }
    }

    public void AddBoneInTail()
    {
        if (_activeBones.Count >= _bones.Count)
            return;

        var addedBone = _bones[_activeBones.Count];
        addedBone.Enable();
        _activeBones.Add(addedBone);

        _tail.transform.SetParent(_activeBones[_activeBones.Count - 1].transform);
    }

    public void RemoveBoneFromTail()
    {
        if (_activeBones.Count <= MinLength)
            return;

        _tail.transform.SetParent(_activeBones[_activeBones.Count - 2].transform);
        _tail.transform.localPosition = Vector3.zero;

        var removedBone = _activeBones[_activeBones.Count - 1];
        removedBone.Disable();
        removedBone.transform.localPosition = Vector3.zero;

        _activeBones.RemoveAt(_activeBones.Count - 1);

        BoneRemoved?.Invoke();
    }

    public void SetInitialTailSize()
    {
        while (_activeBones.Count > MinLength)
            RemoveBoneFromTail();
    }
}
