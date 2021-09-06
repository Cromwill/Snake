using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonShowDelay : MonoBehaviour
{
    [SerializeField] private EndOfGameCanvas _endCanvas;
    [SerializeField] private RewardedButtonActivator _rewardedButtonActivator;
    [SerializeField] private Button _targetButton;

    private void OnEnable()
    {
        _endCanvas.Showed += OnCanvasShowed;
    }

    private void OnDisable()
    {
        _endCanvas.Showed -= OnCanvasShowed;
    }

    private void OnCanvasShowed()
    {
        if (_rewardedButtonActivator.IsActivated)
            StartCoroutine(ShowDelay(2.5f));
        else
            _targetButton.gameObject.SetActive(true);
    }

    private IEnumerator ShowDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _targetButton.gameObject.SetActive(true);
    }
}
