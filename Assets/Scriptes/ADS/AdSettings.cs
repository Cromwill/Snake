using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Events;

public class AdSettings : Singleton
{
    private const int InterstitialDelay = 40;

    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    private BannerView _bannerView;
    private DateTime _lastInterstitialShow;

    public event UnityAction InterstitialShowed;
    public event UnityAction InterstitialShowTryed;
    public event UnityAction RewardedLoaded;
    public event UnityAction UserEarnedReward;

    private void Start()
    {
        _lastInterstitialShow = DateTime.MinValue;

        MobileAds.Initialize(initStatus => { });

        RequestInterstitial();
        RequestRewarded();
        RequestBanner();
    }

    public void ShowInterstitial()
    {
        var delayed = (DateTime.Now - _lastInterstitialShow).Seconds > InterstitialDelay;

        if (_interstitialAd.IsLoaded() && delayed)
            _interstitialAd.Show();
        else
            InterstitialShowTryed?.Invoke();
    }

    public void ShowRewarded()
    {
        if (_rewardedAd.IsLoaded())
            _rewardedAd.Show();
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        _interstitialAd = new InterstitialAd(adUnitId);

        _interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _interstitialAd.OnAdOpening += HandleOnAdOpened;
        _interstitialAd.OnAdClosed += HandleOnAdClosed;
        _interstitialAd.OnAdFailedToShow += HandleOnAdFailedToShow;

        AdRequest request = new AdRequest.Builder().Build();
        _interstitialAd.LoadAd(request);
    }

    private void RequestRewarded()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        _rewardedAd = new RewardedAd(adUnitId);

        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        _rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(request);
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        AdSize bannerSize = new AdSize(320, 50);
        _bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        _bannerView.OnAdLoaded += this.HandleOnBannerLoaded;
        _bannerView.OnAdFailedToLoad += this.HandleOnBannerFailedToLoad;
        _bannerView.OnAdOpening += this.HandleOnBannerOpened;
        _bannerView.OnAdClosed += this.HandleOnBannerClosed;

        AdRequest request = new AdRequest.Builder().Build();
        _bannerView.LoadAd(request);
    }

    #region InterstitialCallbacks
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        _lastInterstitialShow = DateTime.Now;
        InterstitialShowed?.Invoke();
        RequestInterstitial();
        Debug.Log("HandleAdClosed event received");
    }

    public void HandleOnAdFailedToShow(object sender, EventArgs args)
    {
        InterstitialShowTryed?.Invoke();
        Debug.Log("HandleAdLeavingApplication event received");
    }
    #endregion

    #region RewardedCallback
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        RewardedLoaded?.Invoke();
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToLoad event received with message: " + args.LoadAdError.GetMessage());
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.AdError.GetMessage());
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        _lastInterstitialShow = DateTime.Now;
        RequestRewarded();
        Debug.Log("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

        UserEarnedReward?.Invoke();
    }
    #endregion

    #region BannerCallbacks
    public void HandleOnBannerLoaded(object sender, EventArgs args)
    {
        _bannerView.Show();
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.LoadAdError.GetMessage());
    }

    public void HandleOnBannerOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnBannerClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
    } 
    #endregion
}
