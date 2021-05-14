using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public Vector3 GetPosition(float t)
    {
        float delta = t * 0.5f * Mathf.Rad2Deg;

        var posHeight = transform.position + Vector3.down * transform.lossyScale.y + Vector3.up *2 * transform.lossyScale.y * t;

        posHeight += Vector3.forward * Mathf.Cos(delta) * transform.lossyScale.z / 2f;
        posHeight += Vector3.right * Mathf.Sin(delta) * transform.lossyScale.x / 2f;

        return posHeight;
    }
}
