using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class NoAdsButtonActivator : MonoBehaviour
{
    [SerializeField] private IAPButton _targetButton;

    private AdSettings _adSettings;

    private void Awake()
    {
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void OnEnable()
    {
        _adSettings.AdsRemoved += OnAdRemoved;
    }

    private void OnDisable()
    {
        _adSettings.AdsRemoved -= OnAdRemoved;
    }

    private void OnAdRemoved()
    {
        _targetButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (Singleton<AdSettings>.Instance.IsAdsRemove)
            OnAdRemoved();
    }
}
