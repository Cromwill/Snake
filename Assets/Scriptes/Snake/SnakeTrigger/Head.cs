using UnityEngine;
using UnityEngine.Events;

public class Head : SnakeTrigger
{
    [SerializeField] private bool _isPlayerHead;

    public bool IsPlayerHead => _isPlayerHead;

    public event UnityAction<Obstacle> ObstacleEntered;
    public event UnityAction<Food> FoodFinded;

    protected override void TriggerEntering(Collider other)
    {
        var obstacle = other.GetComponent<Obstacle>();

        if (obstacle != null)
            ObstacleEntered?.Invoke(obstacle);
        else if (other.TryGetComponent(out Food food))
            FoodFinded?.Invoke(food);
    }
}