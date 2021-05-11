using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Snake))]
[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeTailCutter : MonoBehaviour
{
    [SerializeField] private SnakeTailStump _stumpTemplate;

    private Snake _snake;
    private SnakeSkeleton _snakeSkeleton;
    private Coroutine _stuckCoroutine;
    private int _tailLengthBeforeCut;

    private void Awake()
    {
        _snake = GetComponent<Snake>();
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
    }

    private void OnEnable()
    {
        _snakeSkeleton.Head.ObstacleEntered += OnObstacleEntered;
    }

    private void OnDisable()
    {
        _snakeSkeleton.Head.ObstacleEntered -= OnObstacleEntered;
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        if (_stuckCoroutine != null)
        {
            _stuckCoroutine = StartCoroutine(StuckCoroutine());
            return;
        }

        var inst = Instantiate(_stumpTemplate, transform.position, Quaternion.identity);
        inst.SetStructure(_snakeSkeleton, _snake.Track, _snake.TrackIndex, _snake.DistanceCovered, _snake.BoneDistance);

        _tailLengthBeforeCut = _snakeSkeleton.ActiveBones.Count;
        _stuckCoroutine = StartCoroutine(StuckCoroutine());
    }

    private IEnumerator StuckCoroutine()
    {
        _snakeSkeleton.SetInitialTailSize();
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < _tailLengthBeforeCut - _snakeSkeleton.MinLength - 2; i++)
            _snakeSkeleton.AddBoneInTail();

        _stuckCoroutine = null;
    }
}
