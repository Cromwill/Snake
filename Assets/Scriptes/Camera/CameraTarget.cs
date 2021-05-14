using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private SnakeSkeleton _followingSnake;

    private void LateUpdate()
    {
        transform.position = _followingSnake.Head.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, _followingSnake.Head.transform.rotation, Time.deltaTime);
    }
}
