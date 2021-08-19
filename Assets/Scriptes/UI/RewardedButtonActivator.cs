using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedButtonActivator : MonoBehaviour
{
    [SerializeField] private GameObject _button;

    private AdSettings _adSettings;

    private void Awake()
    {
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void OnEnable()
    {
        _adSettings.RewardedLoaded += OnRewardedLoaded;
    }

    private void OnDisable()
    {
        _adSettings.RewardedLoaded -= OnRewardedLoaded;
    }

    private void OnRewardedLoaded()
    {
        StartCoroutine(ActivateButton());
    }

    private IEnumerator ActivateButton()
    {
        yield return new WaitForSeconds(1f);
        _button.SetActive(true);
    }
}
