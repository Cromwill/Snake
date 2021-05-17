using UnityEngine;
using UnityEngine.Events;

public class Head : SnakeTrigger
{
    [SerializeField] private bool _isPlayerHead;

    public event UnityAction<Obstacle> ObstacleEntered;
    public event UnityAction<Obstacle> ObstacleExited;
    public event UnityAction<Food> FoodFinded;

    protected override void TriggerEntering(Collider other)
    {
        var obstacleDamager = other.GetComponent<ObstacleDamageComponent>();

        if (obstacleDamager != null)
            ObstacleEntered?.Invoke(obstacleDamager.ParentObstacle);
        else if (other.TryGetComponent(out Food food))
            FoodFinded?.Invoke(food);
    }

    protected override void TriggerExiting(Collider other)
    {
        var obstacleDamager = other.GetComponent<ObstacleDamageComponent>();

        if (obstacleDamager != null)
            ObstacleExited?.Invoke(obstacleDamager.ParentObstacle);
    }
}