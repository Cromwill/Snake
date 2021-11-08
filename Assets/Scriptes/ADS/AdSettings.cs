using System;
using UnityEngine;
using UnityEngine.Events;

public class AdSettings : Singleton<AdSettings>
{
    private const string AppLovinSdkKey = "R5ZeDg0t8rV5BQ4h_72SUwzDKUOipd1Ju_H3yph9eKZV6NZBDqI_rLKZmyFWiyFWdOn4ITSHwMdob2TtWHuzio";
    private const string InterstitialAdId = "94df4f5306ef82e6";
    private const string RewardedAdId = "504d6c9a3f7c0d0a";
    private const string RemoveAdsKey = nameof(RemoveAdsKey);

    private int retryAttempt;
    private const int InterstitialDelay = 30;
    private DateTime _lastInterstitialShow;
    private string _placement;

    public bool IsAdsRemove { get; private set; }
    public bool IsRewardLoad => MaxSdk.IsRewardedAdReady(RewardedAdId);

    public event UnityAction<string, string, string, bool> VideoAdsAviable;
    public event UnityAction<string, string, string, bool> VideoAdsStarted;
    public event UnityAction<string, string, string, bool> VideoAdsWatched;
    public event UnityAction InterstitialShowed;
    public event UnityAction InterstitialShowTryed;
    public event UnityAction RewardedLoaded;
    public event UnityAction UserEarnedReward;
    public event UnityAction AdsRemoved;

    protected override void OnAwake()
    {
        IsAdsRemove = PlayerPrefs.HasKey(RemoveAdsKey);

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => { };
        MaxSdk.SetSdkKey(AppLovinSdkKey);
        MaxSdk.InitializeSdk();

        _lastInterstitialShow = DateTime.MinValue;

        InitializeInterstitialAds();
        InitializeRewardedAds();

        ShowBanner();
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt(RemoveAdsKey, 1);
        IsAdsRemove = true;

        HideBanner();

        AdsRemoved?.Invoke();
    }

    public void ReturnAdsTest()
    {
        PlayerPrefs.DeleteKey(RemoveAdsKey);
        IsAdsRemove = false;
    }

    public void ShowInterstitial(string placement = "")
    {
        if (IsAdsRemove)
        {
            InterstitialShowTryed?.Invoke();
            return;
        }

        var dateDiff = DateTime.Now.Subtract(_lastInterstitialShow);
        var delayed = dateDiff.TotalSeconds > InterstitialDelay;

        _placement = placement;
        if (MaxSdk.IsInterstitialReady(InterstitialAdId) && delayed)
        {
            VideoAdsAviable?.Invoke("interstitial", placement, "success", true);
            MaxSdk.ShowInterstitial(InterstitialAdId);
        }
        else
        {
            VideoAdsAviable?.Invoke("interstitial", placement, "not_aviable", false);
            InterstitialShowTryed?.Invoke();
        }
    }

    public void ShowRewarded(string placement = "")
    {
        _placement = placement;
        if (MaxSdk.IsRewardedAdReady(RewardedAdId))
        {
            VideoAdsAviable?.Invoke("rewarded", placement, "success", true);
            MaxSdk.ShowRewardedAd(RewardedAdId);
        }
        else
        {
            VideoAdsAviable?.Invoke("rewarded", placement, "not_aviable", false);
        }
    }

    public void ShowBanner()
    {
        if (IsAdsRemove)
            return;

        //MaxSdk.ShowBanner(BannerAdId);
    }

    public void HideBanner()
    {
        //MaxSdk.HideBanner(BannerAdId);
    }

    public void InitializeInterstitialAds()
    {
        if (IsAdsRemove)
            return;

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
        //MaxSdk.CreateBanner(BannerAdId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        //MaxSdk.SetBannerBackgroundColor(BannerAdId, Color.white);
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
        _lastInterstitialShow = DateTime.Now;

        VideoAdsStarted?.Invoke("interstitial", _placement, "start", true);
        InterstitialShowed?.Invoke();
        Debug.Log("OnInterstitialDisplayedEvent");
        LoadInterstitial();
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        VideoAdsStarted?.Invoke("interstitial", _placement, "failed_to_display", false);
        InterstitialShowTryed?.Invoke();
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        _lastInterstitialShow = DateTime.Now;
        VideoAdsWatched?.Invoke("interstitial", _placement, "hiding", true);
        LoadInterstitial();
    }
    #endregion

    #region RewardedCallback

    public void LoadRewardedAd()
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
        VideoAdsStarted?.Invoke("rewarded", _placement, "start", true);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        VideoAdsStarted?.Invoke("rewarded", _placement, "failed_to_display", false);
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        _lastInterstitialShow = DateTime.Now;
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        string type = reward.Label;
        double amount = adInfo.Revenue;
        Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

        VideoAdsWatched?.Invoke("interstitial", _placement, "earned_reward", true);
        UserEarnedReward?.Invoke();
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

    #endregion
}