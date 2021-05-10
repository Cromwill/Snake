using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Head : SnakeTrigger
{
    public event UnityAction<Obstacle> ObstacleEntered;
    public event UnityAction<Food> FoodFinded;

    protected override void TriggerEntering(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
            ObstacleEntered?.Invoke(obstacle);
        else if (other.TryGetComponent(out Food food))
            FoodFinded?.Invoke(food);
    }
}