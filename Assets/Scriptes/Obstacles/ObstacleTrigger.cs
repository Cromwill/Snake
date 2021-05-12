using System;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    public event Action TriggerExit;

    private void OnTriggerExit(Collider other)
    {
        Head _otherHead = other.GetComponent<Head>();
        if(_otherHead != null && _otherHead.IsPlayerHead)
        {
            TriggerExit?.Invoke();
        }
    }

}