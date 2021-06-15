using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _confettiEffects;
    [SerializeField] private Transform _spawnerPointParent;

    private List<Vector3> _spawnPositions;
    private Coroutine _spanCoroutine;

    private void Start()
    {
        _spawnPositions = new List<Vector3>();

        for (int i = 0; i < _spawnerPointParent.childCount; i++)
            _spawnPositions.Add(_spawnerPointParent.GetChild(i).position);
    }

    public void StartSpawn()
    {
        if (_spanCoroutine != null)
            StopCoroutine(_spanCoroutine);

        _spanCoroutine = StartCoroutine(SpawnCoroutine());
    }

    public void StopSpawn()
    {
        if (_spanCoroutine == null)
            return;

        StopCoroutine(_spanCoroutine);
        _spanCoroutine = null;
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            var randomEffect = _confettiEffects[Random.Range(0, _confettiEffects.Length)];
            var randomPosition = _spawnPositions[Random.Range(0, _spawnPositions.Count)];
            var randomDelay = Random.Range(0.5f, 2f);

            Instantiate(randomEffect, randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}
