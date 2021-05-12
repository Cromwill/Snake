using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinMaterialColorShaker : MonoBehaviour
{
    private Material _material;
    private Color _startColor;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        _material = GetComponent<SkinnedMeshRenderer>().material;
    }

    private void Start()
    {
        _startColor = _material.color;
    }

    public void Shake(Color color, int shakeCount)
    {
        //Debug.Log("Shake " + name);
        //if (_shakeCoroutine != null)
        //{
        //    StopCoroutine(_shakeCoroutine);
        //    _material.color = _startColor;
        //}

        //_shakeCoroutine = StartCoroutine(ShakeCoroutine(color, shakeCount));
    }

    private IEnumerator ShakeCoroutine(Color color, int shakeCount)
    {
        for (int i = 0; i < shakeCount; i++)
        {
            while (_material.color != color)
            {
                _material.color = Color.LerpUnclamped(_material.color, color, 100f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            while (_material.color != _startColor)
            {
                _material.color = Color.LerpUnclamped(_material.color, _startColor, 100f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
