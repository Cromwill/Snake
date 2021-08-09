using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class BonusDiamondCollectedPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text _diamondValue;

    private Animator _selfAnimator;
    private Coroutine _updateValueCoroutne;
    private int _currentValue;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    public void UpdateCollectedValue(int collectedValue)
    {
        if (_updateValueCoroutne != null)
            StopCoroutine(_updateValueCoroutne);

        _updateValueCoroutne = StartCoroutine(SmoothUpdateValue(collectedValue));
    }

    private IEnumerator SmoothUpdateValue(int targetValue)
    {
        var delay = new WaitForSeconds(0.3f / (targetValue - _currentValue));

        while(_currentValue < targetValue)
        {
            _currentValue++;
            _diamondValue.text = _currentValue.ToString();
            yield return delay;
        }
    }
}
