using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShower : MonoBehaviour
{
    [SerializeField] private GameObject _cupGroup;
    [SerializeField] private SnakeInitializer _snakeInitaizlizer;

    private SnakeBoneMovement _snakeBoneMovement;
    private Pole _pole;

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (_pole == null)
            _pole = FindObjectOfType<Pole>();
        _cupGroup.transform.position = _pole.transform.position + Vector3.up * _pole.transform.lossyScale.y;
    }

#endif

    private void OnEnable()
    {
        _snakeInitaizlizer.Initialized += OnSnakeInitizlised;
    }

    private void OnDisable()
    {
        if (_snakeBoneMovement)
            _snakeBoneMovement.Full—rawled -= OnPlayerFullCrawled;
    } 

    private void OnSnakeInitizlised(Snake snake)
    {
        _snakeBoneMovement = snake.GetComponent<SnakeBoneMovement>();
        _snakeBoneMovement.Full—rawled += OnPlayerFullCrawled;
    }

    private void OnPlayerFullCrawled()
    {
        _cupGroup.SetActive(true);
    }
}
