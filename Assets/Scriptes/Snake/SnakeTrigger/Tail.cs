using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : SnakeTrigger
{
    public void LateUpdate()
    {
        Vector3 parentScale = transform.parent.lossyScale;

        if (parentScale.x < 0.1f || parentScale.y < 0.1f || parentScale.z < 0.1f)
            return;

        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(1f / parentScale.x, .1f / parentScale.y, 1f / parentScale.z);
    }
}
