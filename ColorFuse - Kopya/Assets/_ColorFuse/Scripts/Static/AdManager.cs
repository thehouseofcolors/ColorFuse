

using System.Threading.Tasks;
using UnityEngine;
using System;

public class AdManager : Singleton<AdManager>
{

    [Header("Ad Settings")]
    [SerializeField] private float _minTimeBetweenInterstitials = 60f;
    [SerializeField] private bool _testMode = true;

    private DateTime _lastInterstitialTime = DateTime.MinValue;
    private bool _isShowingAd = false;

    private void Awake()
    {

        InitializeAdSDK();
    }

    private void InitializeAdSDK()
    {
        // Initialize your ad network here (Unity Ads, AdMob, etc.)
        if (_testMode)
        {
            Debug.Log("Ad SDK initialized in test mode");
        }
    }
    public async Task ShowAdAndContinue()
    {
        GameTimer.Instance.Pause();

        await ShowRewardedAdAsync("");

        GameTimer.Instance.Resume();
    }

    public async Task<bool> ShowInterstitialAdAsync()
    {
        if (_isShowingAd) return false;

        // Check if enough time has passed since last interstitial
        if ((DateTime.Now - _lastInterstitialTime).TotalSeconds < _minTimeBetweenInterstitials)
        {
            Debug.Log($"Interstitial ad skipped. Minimum {_minTimeBetweenInterstitials} seconds between ads.");
            return false;
        }

        _isShowingAd = true;

        try
        {
            Debug.Log("Showing interstitial ad...");

            // Simulate ad loading and showing
            await Task.Delay(1000); // Replace with actual ad loading

            if (_testMode)
            {
                Debug.Log("[TEST] Interstitial ad shown successfully");
            }

            _lastInterstitialTime = DateTime.Now;
            return true;
        }
        finally
        {
            _isShowingAd = false;
        }
    }

    public async Task<bool> ShowRewardedAdAsync(string placementId, Action onReward = null)
    {
        if (_isShowingAd) return false;

        _isShowingAd = true;

        try
        {
            Debug.Log($"Showing rewarded ad for placement: {placementId}");

            // Simulate ad loading and showing
            await Task.Delay(1500); // Replace with actual ad loading

            // In test mode, always reward for testing purposes
            bool rewarded = _testMode || UnityEngine.Random.Range(0, 4) > 0; // 75% success rate

            if (rewarded)
            {
                Debug.Log($"Rewarded ad completed for {placementId}");
                onReward?.Invoke();
                return true;
            }
            else
            {
                Debug.Log("User skipped or didn't complete the ad");
                return false;
            }
        }
        finally
        {
            _isShowingAd = false;
        }
    }

    // For backwards compatibility with existing code
    public static void ShowRewardedAd(Action onReward)
    {
        _ = Instance.ShowRewardedAdAsync("default", onReward);
    }
}

