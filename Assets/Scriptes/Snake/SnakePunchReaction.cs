using System.Collections;
using UnityEngine;

public class SnakePunchReaction : MonoBehaviour
{
    [SerializeField] private ParticleSystem _punchEffectTemplate;

    private Animator _selfAnimator;
    private Snake _snake;
    private SnakeSkeleton _snakeSkeleton;
    private Coroutine _punchCoroutine;

    private void Awake()
    {
        _snake = GetComponent<Snake>();
        _snakeSkeleton = GetComponent<SnakeSkeleton>();
        _selfAnimator = GetComponentInChildren<Animator>();
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
        obstacle.DisableObstacle();

        if (_punchCoroutine != null)
        {
            StopCoroutine(_punchCoroutine);
            _punchCoroutine = StartCoroutine(PunchCoroutine());
            return;
        }

        _punchCoroutine = StartCoroutine(PunchCoroutine());
        _snakeSkeleton.RemoveBoneFromTail();

        Instantiate(_punchEffectTemplate, _snakeSkeleton.Head.transform.position + Vector3.up, Quaternion.identity);
    }

    private IEnumerator PunchCoroutine()
    {
        _snake.SetSpeedRate(0.0f);
        yield return new WaitForSeconds(1f);

        _snake.SetSpeedRate(1f);
        _punchCoroutine = null;
    }
}