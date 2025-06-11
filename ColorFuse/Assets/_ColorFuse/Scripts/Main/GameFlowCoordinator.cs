
// // Kullanıcı butona basar → LevelSelectedEvent
// // ↓
// // GameFlowCoordinator → LevelLoadStartedEvent yayınlar
// // ↓
// // LevelDataLoader → LoadLevelData
// // ↓
// // GridSpawner → Grid hazırlar
// // ↓
// // GridSpawnedEvent → UIManager, CameraController tetiklenir
// // ↓
// // UI shows "Level Start" → LevelReadyEvent
// // ↓
// // PlayerInput açılır


// public enum UIPanelType
// {
//     MainMenu,
//     InGame,
//     Win,
//     Fail
// }

// public enum GameState
// {
//     MainMenu,
//     InGame,
//     Win,
//     Fail
// }

// public class GameFlowCoordinator
// {
//     private GameState currentState;

//     public void StartGame()
//     {
//         currentState = GameState.InGame;
//         EventBus.Publish(new LevelStartRequestedEvent());
//         EventBus.Publish(new ShowPanelRequestEvent(UIPanelType.InGame));
//     }

//     public void WinGame()
//     {
//         currentState = GameState.Win;
//         EventBus.Publish(new LevelWonEvent());
//         EventBus.Publish(new ShowPanelRequestEvent(UIPanelType.Win));
//     }

//     public void FailGame()
//     {
//         currentState = GameState.Fail;
//         EventBus.Publish(new LevelFailedEvent());
//         EventBus.Publish(new ShowPanelRequestEvent(UIPanelType.Fail));
//     }

//     public void GoToMainMenu()
//     {
//         currentState = GameState.MainMenu;
//         EventBus.Publish(new ShowPanelRequestEvent(UIPanelType.MainMenu));
//     }
// }
