using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Distance AI", menuName = "Enemy/AI/NewDistanceAI", order = 51)]
public class DistanceAI : EnemyAI
{
    [SerializeField] private float _maxDistance;

    private Coroutine _moveCoroutine;

    public override void StartAI()
    {
        if (_moveCoroutine != null)
            PlayerSnake.StopCoroutine(_moveCoroutine);

        _moveCoroutine = PlayerSnake.StartCoroutine(MoveCoroutine());
    }

    public override void StopAI()
    {
        if (_moveCoroutine != null)
        {
            PlayerSnake.StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            var distanceToPlayer = Vector3.Distance(EnemySnake.HeadTransform.position, PlayerSnake.HeadTransform.position);
            if (distanceToPlayer > _maxDistance && EnemySnake.DistanceCovered < PlayerSnake.DistanceCovered)
            {
                EnemySnake.StartMove();
                var randomMoveTime = Random.Range(1, 3);
                yield return new WaitForSeconds(randomMoveTime);
                EnemySnake.EndMove();
                yield return new WaitForSeconds(Random.Range(0, 100) / 100f);
            }
            else
                yield return new WaitForSeconds(Random.Range(0, 100) / 100f);
        }
    }
}
