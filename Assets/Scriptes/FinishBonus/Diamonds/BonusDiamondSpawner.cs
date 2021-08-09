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
    [Header("Left pole hide positions")]
    [SerializeField] private List<HideInfo> _leftHideInfo;
    [SerializeField] private List<HideInfo> _rightHideInfo;

    private BonusDiamondCollector _diamondCollector;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_bonusFinish != null && Application.isPlaying == false)
        {
            var startSpawnParameter = _bottomSpawnOffset / _bonusFinish.DistanceLength;
            var endSpawnParameter = 1f - _topSpawnOffset / _bonusFinish.DistanceLength;

            DrawDiamonds(_leftPole, startSpawnParameter, endSpawnParameter, _leftHideInfo);
            DrawDiamonds(_rightPole, startSpawnParameter, endSpawnParameter, _rightHideInfo);
        }
    }

    private void DrawDiamonds(BonusPole pole, float startSpawnParameter, float endSpawnParameter, List<HideInfo> hideInfo)
    {
        if (pole == null)
            return;

        var leftForwardPositions = pole.GetAllForwardPositions(startSpawnParameter, endSpawnParameter).ToList();

        for (int i = 0; i < leftForwardPositions.Count; i++)
        {
            var isHide = hideInfo.Any(info => i >= info.Fromindex && i <= info.ToIndex);

            if (isHide)
                continue;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(leftForwardPositions[i], 1f);
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
        var endSpawnParameter = 1f -_topSpawnOffset / _bonusFinish.DistanceLength;

        var leftDiamods = SpawnDiamonds(_leftPole, startSpawnParameter, endSpawnParameter, _leftHideInfo);
        var rightDiamods = SpawnDiamonds(_rightPole, startSpawnParameter, endSpawnParameter, _rightHideInfo);

        _diamondCollector.Init(leftDiamods.Concat(rightDiamods));
    }

    private IEnumerable<BonusDiamond> SpawnDiamonds(BonusPole pole, float startSpawnParameter, float endSpawnParameter, List<HideInfo> hideInfo)
    {
        var spawnedDiamonds = new List<BonusDiamond>();
        var leftForwardPositions = pole.GetAllForwardPositions(startSpawnParameter, endSpawnParameter).ToList();

        for (int i = 0; i < leftForwardPositions.Count; i++)
        {
            var isHide = hideInfo.Any(info => i >= info.Fromindex && i <= info.ToIndex);

            if (isHide)
                continue;

            var inst = Instantiate(_diamondTemplate, leftForwardPositions[i], _diamondTemplate.transform.rotation, transform);
            spawnedDiamonds.Add(inst);
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
    }
}