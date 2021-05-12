using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Food : MonoBehaviour
{
    private Collider _collider;

    public Vector3 ColliderCenterPosition => _collider.bounds.center;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }
}
