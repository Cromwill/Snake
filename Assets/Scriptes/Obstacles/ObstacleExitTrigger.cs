using System;
using UnityEngine;

public class ObstacleExitTrigger : MonoBehaviour
{
    [SerializeField] private Obstacle _obstacle;

    public event Action TriggerExit;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Head head))
        {
            TriggerExit?.Invoke();
            if (_obstacle != null)
                _obstacle.DisableObstacle();
        }
    }
}