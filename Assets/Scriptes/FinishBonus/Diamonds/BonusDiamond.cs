using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BonusDiamond : MonoBehaviour
{
    public int Cost => 10;

    public event UnityAction<BonusDiamond> Collected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Head head))
        {
            Collected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
