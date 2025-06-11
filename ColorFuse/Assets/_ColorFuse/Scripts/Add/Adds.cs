
// using UnityEngine;


// public interface IAdService
// {
//     void ShowRewardedAd(Action<bool> onComplete); // bool: success
//     void ShowInterstitialAd(Action onComplete);
//     bool IsRewardedAdAvailable { get; }
    
// }

// public class DummyAdService : IAdService
// {
//     public void ShowRewardedAd(Action<bool> onComplete)
//     {
//         Debug.Log("Simulated Ad Playing...");
//         onComplete?.Invoke(true);
//     }

//     public void ShowInterstitialAd(Action onComplete)
//     {
//         Debug.Log("Simulated Interstitial...");
//         onComplete?.Invoke();
//     }

//     public bool IsRewardedAdAvailable => true;
// }
