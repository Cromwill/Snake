using System;
using UnityEngine;
using UnityEngine.Events;

public class AdSettings : Singleton<AdSettings>
{
    private const string AppLovinSdkKey = "R5ZeDg0t8rV5BQ4h_72SUwzDKUOipd1Ju_H3yph9eKZV6NZBDqI_rLKZmyFWiyFWdOn4ITSHwMdob2TtWHuzio";
    private const string InterstitialAdId = "071b3528f77ae7f4";
    private const string RewardedAdId = "cfaea1bc2df062fb";
    private int retryAttempt;

    private const int InterstitialDelay = 40;

    private DateTime _lastInterstitialShow;

    public event UnityAction InterstitialShowed;
    public event UnityAction InterstitialShowTryed;
    public event UnityAction RewardedLoaded;
    public event UnityAction UserEarnedReward;

    private void Awake()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => { };
        MaxSdk.SetSdkKey(AppLovinSdkKey);
        MaxSdk.InitializeSdk();

        _lastInterstitialShow = DateTime.MinValue;

        //InitializeBannerAds();
        InitializeInterstitialAds();
        //InitializeRewardedAds();

        //MaxSdk.SetUserId("USER_ID");
    }

    public void ShowInterstitial()
    {
        var dateDiff = DateTime.Now.Subtract(_lastInterstitialShow);
        var delayed = dateDiff.TotalSeconds > InterstitialDelay;

        if (MaxSdk.IsInterstitialReady(InterstitialAdId) && delayed)
            MaxSdk.ShowInterstitial(InterstitialAdId);
        else
            InterstitialShowTryed?.Invoke();
    }

    public void ShowRewarded()
    {
        if (MaxSdk.IsRewardedAdReady(RewardedAdId))
            MaxSdk.ShowRewardedAd(RewardedAdId);
    }

    public void ShowBanner()
    {
        //MaxSdk.ShowBanner(_adUnitId);
    }

    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        //MaxSdk.CreateBanner(_adUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        //MaxSdk.SetBannerBackgroundColor(_adUnitId, Color.white);
    }

    #region InterstitialCallbacks

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(InterstitialAdId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        InterstitialShowTryed?.Invoke();
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        _lastInterstitialShow = DateTime.Now;
        InterstitialShowed?.Invoke();

        LoadInterstitial();
    }
    #endregion

    #region RewardedCallback

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        // Reset retry attempt
        RewardedLoaded?.Invoke();
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        _lastInterstitialShow = DateTime.Now;
        Debug.Log("HandleRewardedAdClosed event received");
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        string type = reward.Label;
        double amount = adInfo.Revenue;
        Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

        UserEarnedReward?.Invoke();
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

    #endregion
}
