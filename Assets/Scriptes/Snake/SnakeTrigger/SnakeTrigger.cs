using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeTrigger : MonoBehaviour
{
    public event UnityAction<Collider> TriggerEntered;
    public event UnityAction<Collider> TriggerExited;

    private void OnTriggerEnter(Collider other)
    {
        TriggerEntering(other);
        TriggerEntered?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExiting(other);
        TriggerExited?.Invoke(other);
    }

    protected virtual void TriggerEntering(Collider other) { }
    protected virtual void TriggerExiting(Collider other) { }
}
