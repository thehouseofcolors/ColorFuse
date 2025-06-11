// using UnityEngine;

// public class EventLogHandler : MonoBehaviour
// {
//     public void OnEnable()
//     {
//         // Level Events
//         EventBus.Subscribe<LevelEvents.LevelCompletedEvent>(OnLevelCompleted);
//         EventBus.Subscribe<LevelEvents.LevelFailedEvent>(OnLevelFailed);
//         EventBus.Subscribe<LevelEvents.LevelSuccessEvent>(OnLevelSuccess);
//         EventBus.Subscribe<LevelEvents.LevelUnlockedEvent>(OnLevelUnlocked);
//         EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);

//         // UI Events
//         EventBus.Subscribe<UIEvents.ShowMainMenuEvent>(OnMainMenuShown);

//         // Undo Events
//         EventBus.Subscribe<UndoEvents.UndoActionRegisteredEvent>(OnUndoRegistered);
//         EventBus.Subscribe<UndoEvents.UndoPerformedEvent>(OnUndoPerformed);
//         EventBus.Subscribe<UndoEvents.UndoRequestedEvent>(OnUndoRequested);
//         EventBus.Subscribe<UndoEvents.UndoStackClearedEvent>(OnUndoCleared);
//         EventBus.Subscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoUsageUpdated);

//         // Tile Events
//         EventBus.Subscribe<TileEvents.ColorCombinedEvent>(OnColorCombined);
//         EventBus.Subscribe<TileEvents.WhiteColorFormedEvent>(OnWhiteColorFormed);
//         EventBus.Subscribe<TileEvents.TileStateChangedEvent>(OnTileStateChanged);

//         // Yeni eklenen shuffle eventleri:
//         EventBus.Subscribe<ShuffleEvents.ShuffledEvent>(OnShuffled);
//         EventBus.Subscribe<ShuffleEvents.ShuffleRequestedEvent>(OnShuffleRequested);
//         EventBus.Subscribe<ShuffleEvents.ShuffleUsageUpdatedEvent>(OnShuffleUsageUpdated);

//     }

//     public void OnDisable()
//     {
//         EventBus.Unsubscribe<LevelEvents.LevelCompletedEvent>(OnLevelCompleted);
//         EventBus.Unsubscribe<LevelEvents.LevelFailedEvent>(OnLevelFailed);
//         EventBus.Unsubscribe<LevelEvents.LevelSuccessEvent>(OnLevelSuccess);
//         EventBus.Unsubscribe<LevelEvents.LevelUnlockedEvent>(OnLevelUnlocked);
//         EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);

//         EventBus.Unsubscribe<UIEvents.ShowMainMenuEvent>(OnMainMenuShown);

//         EventBus.Unsubscribe<UndoEvents.UndoActionRegisteredEvent>(OnUndoRegistered);
//         EventBus.Unsubscribe<UndoEvents.UndoPerformedEvent>(OnUndoPerformed);
//         EventBus.Unsubscribe<UndoEvents.UndoRequestedEvent>(OnUndoRequested);
//         EventBus.Unsubscribe<UndoEvents.UndoStackClearedEvent>(OnUndoCleared);
//         EventBus.Unsubscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoUsageUpdated);

//         EventBus.Unsubscribe<TileEvents.ColorCombinedEvent>(OnColorCombined);
//         EventBus.Unsubscribe<TileEvents.WhiteColorFormedEvent>(OnWhiteColorFormed);
//         EventBus.Unsubscribe<TileEvents.TileStateChangedEvent>(OnTileStateChanged);

//         // Yeni eklenen shuffle eventleri:
//         EventBus.Unsubscribe<ShuffleEvents.ShuffledEvent>(OnShuffled);
//         EventBus.Unsubscribe<ShuffleEvents.ShuffleRequestedEvent>(OnShuffleRequested);
//         EventBus.Unsubscribe<ShuffleEvents.ShuffleUsageUpdatedEvent>(OnShuffleUsageUpdated);

//     }

//     // ----------- LEVEL EVENTS -------------
//     private void OnLevelStarted(LevelEvents.LevelStartedEvent e)
//     {
//         Debug.Log($"[EventLog] Level Started: {e.Level}, IsRestart: {e.IsRestart}");
//     }

//     private void OnLevelCompleted(LevelEvents.LevelCompletedEvent e)
//     {
//         Debug.Log($"[EventLog] Level Completed: {e.LevelNumber}, Time: {e.TimeRecord:F2}, Success: {e.IsSuccess}");
//     }

//     private void OnLevelFailed(LevelEvents.LevelFailedEvent e)
//     {
//         Debug.Log("[EventLog] Level Failed");
//     }

//     private void OnLevelSuccess(LevelEvents.LevelSuccessEvent e)
//     {
//         Debug.Log("[EventLog] Level Success");
//     }

//     private void OnLevelUnlocked(LevelEvents.LevelUnlockedEvent e)
//     {
//         Debug.Log($"[EventLog] Level Unlocked: {e.LevelNumber}");
//     }

//     // ----------- UI EVENTS -------------
//     private void OnMainMenuShown(UIEvents.ShowMainMenuEvent e)
//     {
//         Debug.Log("[EventLog] Main Menu Shown");
//     }

//     // ----------- UNDO EVENTS -------------
//     private void OnUndoRegistered(UndoEvents.UndoActionRegisteredEvent e)
//     {
//         Debug.Log("[EventLog] Undo Action Registered");
//     }

//     private void OnUndoPerformed(UndoEvents.UndoPerformedEvent e)
//     {
//         Debug.Log("[EventLog] Undo Performed");
//     }

//     private void OnUndoRequested(UndoEvents.UndoRequestedEvent e)
//     {
//         Debug.Log("[EventLog] Undo Requested");
//     }

//     private void OnUndoCleared(UndoEvents.UndoStackClearedEvent e)
//     {
//         Debug.Log("[EventLog] Undo Stack Cleared");
//     }

//     private void OnUndoUsageUpdated(UndoEvents.UndoUsageUpdatedEvent e)
//     {
//         Debug.Log($"[EventLog] Undo Usage Updated. Remaining: ");
//     }

//     // ----------- TILE EVENTS -------------
//     private void OnColorCombined(TileEvents.ColorCombinedEvent e)
//     {
//         Debug.Log($"[EventLog] Color Combined: {e.SourceTile.name} + {e.TargetTile.name} = {e.Result}");
//     }

//     private void OnWhiteColorFormed(TileEvents.WhiteColorFormedEvent e)
//     {
//         Debug.Log($"[EventLog] White Color Formed at Tile: {e.TargetTile.name}");
//     }

//     private void OnTileStateChanged(TileEvents.TileStateChangedEvent e)
//     {
//         Debug.Log("[EventLog] Tile State Changed");
//     }
    
//     // Shuffle event handlerlar

//     private void OnShuffled(ShuffleEvents.ShuffledEvent e)
//     {
//         Debug.Log("[EventLog] Shuffle completed.");
//     }

//     private void OnShuffleRequested(ShuffleEvents.ShuffleRequestedEvent e)
//     {
//         Debug.Log("[EventLog] Shuffle requested.");
//     }

//     private void OnShuffleUsageUpdated(ShuffleEvents.ShuffleUsageUpdatedEvent e)
//     {
//         Debug.Log("[EventLog] Shuffle usage updated.");
//     }
// }
