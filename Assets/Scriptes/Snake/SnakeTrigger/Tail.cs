using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : SnakeTrigger
{
    public void LateUpdate()
    {
        Vector3 parentScale = transform.parent.lossyScale;

        if (parentScale.x == 0 || parentScale.y == 0 || parentScale.z == 0)
            return;

        transform.localScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);
    }
}
