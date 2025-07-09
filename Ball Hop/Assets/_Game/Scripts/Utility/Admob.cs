using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Admob : Singleton<Admob>
{
    // [SerializeField] [TextArea(1, 2)] string testAppId = "ca-app-pub-3940256099942544~3347511713";
    // [Space(order = 0)]

    [Header("Admob Ad Units :", order = 1)]
    [SerializeField] [TextArea(1, 2)] string idBanner = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] [TextArea(1, 2)] string idInterstitial = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] [TextArea(1, 2)] string idReward = "ca-app-pub-3940256099942544/5224354917";

    [Space]
    [SerializeField] AdPosition bannerAdPosition = AdPosition.Bottom;

    [Header("Toggle Admob Ads :")]
    [SerializeField] bool bannerAdEnabled = true;
    [SerializeField] bool interstitialAdEnabled = true;
    [SerializeField] bool rewardedAdEnabled = true;

    [HideInInspector] public BannerView AdBanner;
    [HideInInspector] public InterstitialAd AdInterstitial;
    [HideInInspector] public RewardedAd AdReward;

    bool initDone = false;

    public static event Action OnRewardedAdWatched;

    public bool IsRewardedAdLoaded => rewardedAdEnabled && AdReward.IsLoaded() ? true : false;

    private void OnEnable() => SceneManager.sceneLoaded += HandleOnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= HandleOnSceneLoaded;

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode lsm)
    {
        if (scene.buildIndex == 0) Destroy(gameObject);
        else if (initDone) ShowBanner();
    }

    private void Start()
    {
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
                .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
                .build();

        MobileAds.Initialize(initstatus =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                ShowBanner();
                RequestRewardAd();
                RequestInterstitialAd();

                initDone = true;
            });
        });
    }

    private void OnDestroy()
    {
        DestroyBannerAd();
        DestroyInterstitialAd();
    }

    AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
           .TagForChildDirectedTreatment(false)
           .AddExtra("npa", PlayerPrefs.GetInt("npa", 1).ToString())
           .Build();
    }

    #region Banner Ad ------------------------------------------------------------------------------
    public void ShowBanner()
    {
        if (!bannerAdEnabled) return;
        DestroyBannerAd();

        AdSize adSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        AdBanner = new BannerView(idBanner, adSize, bannerAdPosition);

        AdBanner.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (AdBanner != null)
        {
            AdBanner.Destroy();
        }
    }
    #endregion

    #region Interstitial Ad ------------------------------------------------------------------------
    public void RequestInterstitialAd()
    {
        AdInterstitial = new InterstitialAd(idInterstitial);

        AdInterstitial.OnAdClosed += HandleInterstitialAdClosed;

        AdInterstitial.LoadAd(CreateAdRequest());
    }

    private void HandleInterstitialAdClosed(object sender, EventArgs e)
    {
        DestroyInterstitialAd();
        RequestInterstitialAd();
    }

    public void ShowInterstitialAd()
    {
        if (!interstitialAdEnabled) return;

        if (AdInterstitial.IsLoaded())
            AdInterstitial.Show();
    }

    public void DestroyInterstitialAd()
    {
        if (AdInterstitial != null)
            AdInterstitial.Destroy();
    }
    #endregion

    #region Rewarded Ad ----------------------------------------------------------------------------
    public void RequestRewardAd()
    {
        AdReward = new RewardedAd(idReward);

        AdReward.OnAdClosed += HandleOnRewardedAdClosed;
        AdReward.OnUserEarnedReward += HandleOnRewardedAdWatched;

        AdReward.LoadAd(CreateAdRequest());
    }

    public void ShowRewardAd()
    {
        if (!rewardedAdEnabled) return;

        if (AdReward.IsLoaded())
            AdReward.Show();
    }

    private void HandleOnRewardedAdClosed(object sender, EventArgs e)
    {
        RequestRewardAd();
    }

    private void HandleOnRewardedAdWatched(object sender, Reward e)
    {
        OnRewardedAdWatched?.Invoke();
    }
    #endregion
}
