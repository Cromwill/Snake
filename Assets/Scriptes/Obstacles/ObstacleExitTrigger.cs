using System;
using UnityEngine;

public class ObstacleExitTrigger : MonoBehaviour
{
    public event Action TriggerExit;

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Head head))
            TriggerExit?.Invoke();
    }
}