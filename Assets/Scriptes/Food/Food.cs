using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }
}
