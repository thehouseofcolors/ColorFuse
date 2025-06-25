
// using UnityEngine;
// using UnityEngine.UI;
// using System.Threading.Tasks;

// public class StartButton : MonoBehaviour
// {
//     [SerializeField] private Button startButton;
//     private LevelConfig selectedLevel;

//     private void Awake()
//     {
//         startButton.onClick.AddListener(OnStartClicked);
//     }

//     private async void OnStartClicked()
//     {
//         if (selectedLevel == null)
//         {
//             Debug.LogWarning("No level selected!");
//             return;
//         }
        
//         await GameStateMachine.ChangeStateAsync(new GamePlayState(selectedLevel));
//     }
// }
