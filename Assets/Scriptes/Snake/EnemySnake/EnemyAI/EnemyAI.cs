using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : ScriptableObject
{
    public Snake EnemySnake { get; private set; }
    public Snake PlayerSnake { get; private set; }

    public void Init(Snake enemySnake, Snake playerSnake)
    {
        EnemySnake = enemySnake;
        PlayerSnake = playerSnake;
    }

    public abstract void StartAI();
    public abstract void StopAI();
}