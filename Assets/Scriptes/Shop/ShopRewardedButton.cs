using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopRewardedButton : MonoBehaviour
{
    private const int RewardedValue = 500;

    private Button _button;
    private AdSettings _adSettings;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
        _adSettings.UserEarnedReward += OnUserEarnedReward;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
        _adSettings.UserEarnedReward -= OnUserEarnedReward;
    }

    private void OnButtonClicked()
    {
        _adSettings.ShowRewarded();
    }

    private void OnUserEarnedReward()
    {
        GemBalance gems = new GemBalance();
        gems.Load(new JsonSaveLoad());
        gems.Add(RewardedValue);
        gems.Save(new JsonSaveLoad());
    }
}
