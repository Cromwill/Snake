using UnityEngine;
using UnityEngine.Events;

public class Head : SnakeTrigger
{
    [SerializeField] private bool _isPlayerHead;

    public bool IsPlayerHead => _isPlayerHead;

    public event UnityAction<Obstacle> ObstacleEntered;
    public event UnityAction<Obstacle> ObstacleExited;
    public event UnityAction<Food> FoodFinded;

    protected override void TriggerEntering(Collider other)
    {
        var obstacle = other.GetComponent<ObstacleDamager>();

        if (obstacle != null)
            ObstacleEntered?.Invoke(obstacle.ParentObstacle);
        else if (other.TryGetComponent(out Food food))
            FoodFinded?.Invoke(food);
    }

    protected override void TriggerExiting(Collider other)
    {
        if (other.TryGetComponent(out ObstacleDamager obstacle))
            ObstacleExited?.Invoke(obstacle.ParentObstacle);
    }
}