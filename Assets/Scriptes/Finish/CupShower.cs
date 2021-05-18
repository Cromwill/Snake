using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShower : MonoBehaviour
{
    [SerializeField] private GameObject _cupGroup;
    [SerializeField] private SnakeBoneMovement _snakeBoneMovement;

    private void OnEnable()
    {
        _snakeBoneMovement.Full—rawled += OnPlayerFullCrawled;
    }

    private void OnDisable()
    {
        _snakeBoneMovement.Full—rawled -= OnPlayerFullCrawled;
    }

    private void OnPlayerFullCrawled()
    {
        _cupGroup.SetActive(true);
    }
}
