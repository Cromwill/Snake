using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoneStretching : MonoBehaviour
{
    [SerializeField] private float _deltaSegments;

    public List<float> Distances => _boneDistances;

    private Coroutine _startStretching;
    private Coroutine _stopStretching;
    private List<float> _boneDistances;
    private float _minDistance;
    private float _stretchingSpeed;

    public void Init(int boneCount, float minDistance, float _snakeSpeed)
    {
        _boneDistances = new List<float>();
        for (int i = 0; i < boneCount; i++)
            _boneDistances.Add(minDistance);

        _minDistance = minDistance;
        _stretchingSpeed = _snakeSpeed / 3f;
    }

    public void StartStretching()
    {
        DisableStretchingCoroutines();

        _startStretching = StartCoroutine(StartStretchingCoroutine());
    }

    public void StopStretching()
    {
        DisableStretchingCoroutines();

        _stopStretching = StartCoroutine(StopStretchingCoroutine());
    }

    private IEnumerator StartStretchingCoroutine()
    {
        for (int i = 0; i < _boneDistances.Count; i++)
        {
            while (Mathf.Abs(_boneDistances[i] - (_minDistance + _deltaSegments)) > 0.001f)
            {
                _boneDistances[i] = Mathf.MoveTowards(_boneDistances[i], _minDistance + _deltaSegments, _stretchingSpeed * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        _startStretching = null;
    }

    private IEnumerator StopStretchingCoroutine()
    {
        bool allBonesStretched = false;
        while (allBonesStretched == false)
        {
            allBonesStretched = true;
            for (int i = 0; i < _boneDistances.Count; i++)
            {
                _boneDistances[i] = Mathf.MoveTowards(_boneDistances[i], _minDistance, _stretchingSpeed * 0.15f * Time.deltaTime);

                if (_boneDistances[i] != _minDistance)
                    allBonesStretched = false;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _stopStretching = null;
    }

    private void DisableStretchingCoroutines()
    {
        if (_startStretching != null)
            StopCoroutine(_startStretching);
        if (_stopStretching != null)
            StopCoroutine(_stopStretching);
    }
}
