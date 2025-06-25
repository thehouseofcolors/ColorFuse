// using System.Threading.Tasks;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using GameEvents;

// public class AdRewardButtonsControl : MonoBehaviour
// {
//     [SerializeField] Button addTimeButton, addMoveButton, restartButton;


//     public void OnEnable()
//     {
//         restartButton.onClick.AddListener(OnUse);
//         addTimeButton.onClick.AddListener(OnAddTimePressed);
//         addMoveButton.onClick.AddListener(OnAddMovesPressed);
//     }

//     public void OnDisable()
//     {
//         restartButton.onClick.RemoveAllListeners();
//         addTimeButton.onClick.RemoveAllListeners();
//         addMoveButton.onClick.RemoveAllListeners();
//     }
    
//     async void OnAddTimePressed()
//     {
//         await AdManager.Instance.ShowInterstitialAdAsync();
//         await GameStateMachine.ChangeStateAsync(new GamePlayState(LevelDataManager.GetLevelConfig(PlayerPrefsService.CurrentLevel)));
//     }
    
//     async void OnAddMovesPressed()
//     {
//         await AdManager.Instance.ShowInterstitialAdAsync();
//         await GameStateMachine.ChangeStateAsync(new GamePlayState(LevelDataManager.GetLevelConfig(PlayerPrefsService.CurrentLevel)));
//     }
//     private async void OnUse()
//     {
        
//         await AdManager.Instance.ShowInterstitialAdAsync();
//         await GameStateMachine.ChangeStateAsync(new GamePlayState(LevelDataManager.GetLevelConfig(PlayerPrefsService.CurrentLevel)));
//     }


// }
