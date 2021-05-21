using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Head : SnakeTrigger
{
    [SerializeField] private bool _isPlayerHead;

    public Vector3 ColliderCenter => _collider.bounds.center;

    public event UnityAction<Obstacle> ObstacleEntered;
    public event UnityAction<Obstacle> ObstacleExited;
    public event UnityAction<Food> FoodFinded;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

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