using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Obstacle))]
public class ObstacleStartDelay : MonoBehaviour
{
    [SerializeField] private float _startdelay;

    private Obstacle _obstacle;

    private void Awake()
    {
        _obstacle = GetComponent<Obstacle>();
    }

    private void Start()
    {
        StartCoroutine(StartDelay(_startdelay));
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _obstacle.EnableObstacle();
    }
}
