using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBone : MonoBehaviour
{
    public Vector3 Position { get => transform.position; set => transform.position = value; }
    public Vector3 LocalPosition { get => transform.localPosition;}

    public void Enable()
    {
        transform.localScale = Vector3.one;
    }

    public void Disable()
    {
        transform.localScale = Vector3.zero;
    }

    public void LookRotation(Vector3 forward)
    {
        var rotation = Quaternion.LookRotation(forward, Vector3.up);
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x - 90f, rotation.eulerAngles.y, rotation.eulerAngles.z);
        transform.rotation = rotation;
    }
}
