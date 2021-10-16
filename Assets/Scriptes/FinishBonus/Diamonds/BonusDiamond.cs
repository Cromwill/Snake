using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BonusDiamond : MonoBehaviour
{
    [SerializeField] private ParticleSystem _idleEffect;
    [SerializeField] private ParticleSystem _collectEffect;

    public int Cost => 2;

    public event UnityAction<BonusDiamond> Collected;

    private void Start()
    {
        Instantiate(_idleEffect, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Head head))
        {
            Instantiate(_collectEffect, transform.position, _collectEffect.transform.rotation);
            Collected?.Invoke(this);

            Destroy(gameObject);
        }
    }
}
