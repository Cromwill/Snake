using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : Snake
{
    [SerializeField] private Snake _playerSnake;
    [SerializeField] private EnemyAI _enemyAI;

    protected override void OnStart()
    {
        _enemyAI.Init(this, _playerSnake);
        _enemyAI.StartAI();
    }
}
