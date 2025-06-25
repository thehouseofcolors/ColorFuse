// using System.Threading.Tasks;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using GameEvents;

// public class _MenuButton : MonoBehaviour
// {
//     [SerializeField] Button menuButton;


//     public void OnEnable()
//     {
//         menuButton.onClick.AddListener(OnUse);
//     }

//     public void OnDisable()
//     {
//         menuButton.onClick.RemoveAllListeners();
//     }
    

//     private async void OnUse()
//     {
//         await AdManager.Instance.ShowInterstitialAdAsync();//bazen reklam olabilir bence
//         await GameStateMachine.ChangeStateAsync(new MenuState());
//     }


// }
// public class MenuButton : MonoBehaviour
// {
//     private Button _button;

//     private void Awake()
//     {
//         _button = GetComponent<Button>();
//         _button.onClick.AddListener(OnMenuButtonClicked);
//     }

//     private void OnMenuButtonClicked()
//     {
//         GameStateMachine.ChangeStateAsync(new MenuState()); // kendi state sisteminle değiştir
//     }

//     private void OnDestroy()
//     {
//         _button.onClick.RemoveListener(OnMenuButtonClicked);
//     }
// }
