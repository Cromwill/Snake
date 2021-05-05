using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField] private Track _track;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = _track.GetPosition(1 / _speedTime * Time.deltaTime);
    }
}
