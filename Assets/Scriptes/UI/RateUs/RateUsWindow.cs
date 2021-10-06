using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RateUsWindow : MonoBehaviour
{
    [SerializeField] private Button _nopeButton; 
    [SerializeField] private Button _sureButton;
    [SerializeField] private Button _closeButton;

    public event UnityAction Closed;
    public event UnityAction Successfully;

    private void OnEnable()
    {
        _nopeButton.onClick.AddListener(OnNopeButtonClicked);
        _sureButton.onClick.AddListener(OnSureButtonClicked);
        _closeButton.onClick.AddListener(OnNopeButtonClicked);
    }

    private void OnDisable()
    {
        _nopeButton.onClick.RemoveListener(OnNopeButtonClicked);
        _sureButton.onClick.RemoveListener(OnSureButtonClicked);
        _closeButton.onClick.RemoveListener(OnNopeButtonClicked);
    }

    private void OnNopeButtonClicked()
    {
        Closed?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnSureButtonClicked()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=snakes.way.rush");
#elif UNITY_IOS
        
#endif
        Successfully?.Invoke();

        gameObject.SetActive(false);
    }
}
