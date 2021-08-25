using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

[RequireComponent(typeof(BonusDiamondCollector))]
public class BonusDiamondSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BonusDiamond _diamondTemplate;
    [SerializeField] private BonusFinish _bonusFinish;
    [SerializeField] private BonusPole _leftPole;
    [SerializeField] private BonusPole _rightPole;
    [Header("SpawnParameters")]
    [SerializeField] private float _bottomSpawnOffset;
    [SerializeField] private float _topSpawnOffset;
    [SerializeField] private bool _onlyForward;
    [SerializeField] private float _diamondOffset = 5f;
    [Header("Hide positions")]
    [SerializeField] private bool _reverce;
    [SerializeField] private List<HideInfo> _hideInfo;

    private BonusDiamondCollector _diamondCollector;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_diamondOffset < 1f)
            _diamondOffset = 1f;

        for (int i = 0; i < _hideInfo.Count; i++)
            if (_hideInfo[i].ToIndex < _hideInfo[i].Fromindex)
                _hideInfo[i] = new HideInfo(_hideInfo[i].Fromindex, _hideInfo[i].Fromindex);
    }

    private void OnDrawGizmos()
    {
        if (_bonusFinish != null && Application.isPlaying == false)
        {
            var startSpawnParameter = _bottomSpawnOffset / _bonusFinish.DistanceLength;
            var endSpawnParameter = 1f - _topSpawnOffset / _bonusFinish.DistanceLength;

            DrawDiamonds(_leftPole, startSpawnParameter, endSpawnParameter, _hideInfo, _reverce);
            DrawDiamonds(_rightPole, startSpawnParameter, endSpawnParameter, _hideInfo, !_reverce);
        }
    }

    private void DrawDiamonds(BonusPole pole, float startSpawnParameter, float endSpawnParameter, List<HideInfo> hideInfo, bool reverce = false)
    {
        if (pole == null)
            return;

        List<Vector3> allPositions;
        if (_onlyForward)
            allPositions = pole.GetAllForwardPositions(startSpawnParameter, endSpawnParameter).ToList();
        else
            allPositions = pole.GetAllPosition(_diamondOffset, startSpawnParameter, endSpawnParameter).ToList();

        for (int i = 0; i < allPositions.Count; i++)
        {
            var isHide = hideInfo.Any(info => i >= info.Fromindex && i <= info.ToIndex);
            if (reverce)
                isHide = !isHide;

            if (isHide)
                continue;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(allPositions[i], 1f);
        }
    }
#endif

    private void Awake()
    {
        _diamondCollector = GetComponent<BonusDiamondCollector>();
    }

    private void Start()
    {
        var startSpawnParameter = _bottomSpawnOffset / _bonusFinish.DistanceLength;
        var endSpawnParameter = 1f - _topSpawnOffset / _bonusFinish.DistanceLength;
        Debug.Log("MyDebug: " + startSpawnParameter + " " + endSpawnParameter);
        var leftDiamods = SpawnDiamonds(_leftPole, startSpawnParameter, endSpawnParameter, _hideInfo, _reverce);
        Debug.Log("MyDebug: stawn left");
        var rightDiamods = SpawnDiamonds(_rightPole, startSpawnParameter, endSpawnParameter, _hideInfo, !_reverce);
        Debug.Log("MyDebug: stawn right");

        _diamondCollector.Init(leftDiamods.Concat(rightDiamods));
        Debug.Log("MyDebug: init");
    }

    private IEnumerable<BonusDiamond> SpawnDiamonds(BonusPole pole, float startSpawnParameter, float endSpawnParameter, List<HideInfo> hideInfo, bool reverce = false)
    {
        var spawnedDiamonds = new List<BonusDiamond>();

        List<Vector3> allPositions;
        if (_onlyForward)
            allPositions = pole.GetAllForwardPositions(startSpawnParameter, endSpawnParameter).ToList();
        else
            allPositions = pole.GetAllPosition(_diamondOffset, startSpawnParameter, endSpawnParameter).ToList();
        Debug.Log("MyDebug: all pos");

        for (int i = 0; i < allPositions.Count; i++)
        {
            var isHide = hideInfo.Any(info => i >= info.Fromindex && i <= info.ToIndex);
            if (reverce)
                isHide = !isHide;

            if (isHide)
                continue;

            Debug.Log("MyDebug: before inst");
            var inst = Instantiate(_diamondTemplate, allPositions[i], _diamondTemplate.transform.rotation, transform);
            spawnedDiamonds.Add(inst);
            Debug.Log("MyDebug: after inst");
        }

        return spawnedDiamonds;
    }


    [Serializable]
    public struct HideInfo
    {
        [SerializeField] private int _fromIndex;
        [SerializeField] private int _toIndex;

        public int Fromindex => _fromIndex;
        public int ToIndex => _toIndex;

        public HideInfo(int fromIndex, int toIndex)
        {
            _fromIndex = fromIndex;
            _toIndex = toIndex;
        }
    }
}