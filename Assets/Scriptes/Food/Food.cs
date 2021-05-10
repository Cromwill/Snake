using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Food : MonoBehaviour
{
    private Rigidbody _rigitbody;

    private void Awake()
    {
        _rigitbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigitbody.isKinematic = true;
    }

    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }
}
