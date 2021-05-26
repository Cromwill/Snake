using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FromToNumberAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _targetText;
    [SerializeField] private string _format;
    [SerializeField] private int _fromValue;
    [SerializeField] private int _toValue;
    [SerializeField] private float _speed;

    private Coroutine _animationCoroutine;

    private void OnEnable()
    {
        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);

        _animationCoroutine = StartCoroutine(AnimationCoroutine());
    }

    private IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        int sign = (int)Mathf.Sign(_toValue - _fromValue);
        int number = _fromValue;
        while (true)
        {
            _targetText.text = string.Format(_format, number);
            yield return new WaitForSeconds(1f/_speed);
            
            if (number == _toValue)
                break;
            number += sign;
        }
    }
}
