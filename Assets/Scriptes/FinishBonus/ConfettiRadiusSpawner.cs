using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConfettiRadiusSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _confettiScale;
    [SerializeField] private ParticleSystem[] _confettiEffects;

    private Coroutine _spawnCoroutine;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Selection.activeGameObject == gameObject)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
#endif

    public void StartSpawn()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        while (true)
        {
            var template = _confettiEffects[Random.Range(0, _confettiEffects.Length)];
            var randomPosition = transform.position + Random.insideUnitSphere * _spawnRadius;
            var delay = Random.Range(0.4f, 1.2f);

            var inst = Instantiate(template, randomPosition, Quaternion.identity, transform);
            inst.transform.localScale = Vector3.one * _confettiScale;

            yield return new WaitForSeconds(delay);
        }
    }
}
