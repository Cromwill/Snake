using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Head : SnakeTrigger
{
    public event UnityAction<Obstacle> ObstacleEntered;

    protected override void TriggerEntering(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            ObstacleEntered?.Invoke(obstacle);
        }
    }
}
