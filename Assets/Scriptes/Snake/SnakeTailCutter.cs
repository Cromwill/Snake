using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Snake))]
[RequireComponent(typeof(SnakeSkeleton))]
public class SnakeTailCutter : MonoBehaviour
{
    [SerializeField] private SnakeTailStump _stumpTemplate;
    [SerializeField] private ParticleSystem _bloodPoolGrowing;
    [SerializeField] private ParticleSystem _bloodExplosion;
    [SerializeField] private ParticleSystem _bloodShower;

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

    private void Start()
    {
        StopEffects();
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        if (_stuckCoroutine != null)
        {
            StopCoroutine(_stuckCoroutine);
            _stuckCoroutine = StartCoroutine(StuckCoroutine());
            return;
        }

        var inst = Instantiate(_stumpTemplate, transform.position + Vector3.up, Quaternion.identity);
        inst.SetStructure(_snakeSkeleton, _snake.Track, _snake.TrackIndex, _snake.DistanceCovered, _snake.BoneDistance);

        _tailLengthBeforeCut = _snakeSkeleton.ActiveBones.Count;
        _stuckCoroutine = StartCoroutine(StuckCoroutine());

        _bloodPoolGrowing.Play(true);
        _bloodExplosion.Play(true);
        _bloodShower.Play(true);
    }

    private IEnumerator StuckCoroutine()
    {
        _snake.SetSpeedRate(0.5f);
        _snakeSkeleton.SetInitialTailSize();
        yield return new WaitForSeconds(4f);

        for (int i = 0; i < _tailLengthBeforeCut - _snakeSkeleton.MinLength - 2; i++)
            _snakeSkeleton.AddBoneInTailSmoothly();

        StopEffects();
        _snake.SetSpeedRate(1f);

        _stuckCoroutine = null;
    }

    private void StopEffects()
    {
        _bloodPoolGrowing.Stop(true);
        _bloodExplosion.Stop(true);
        _bloodShower.Stop(true);
    }
}
