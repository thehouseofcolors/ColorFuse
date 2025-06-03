using UnityEngine;

public class LevelManager : Singleton<LevelManager>, IGameSystem
{
    public static readonly string CurrentLevelKey = "CurrentLevel";

    public void Initialize()
    {
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<LevelEvents.LevelCompletedEvent>(OnLevelCompleted);
        Debug.Log("[LevelManager] Initialized and subscribed to level events.");
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<LevelEvents.LevelCompletedEvent>(OnLevelCompleted);
        Debug.Log("[LevelManager] Shutdown and unsubscribed from level events.");
    }

    private void OnLevelStarted(LevelEvents.LevelStartedEvent e)
    {
        Debug.Log($"[LevelManager] Level {e.Level} started. IsRestart: {e.IsRestart}");
    }

    private void OnLevelCompleted(LevelEvents.LevelCompletedEvent e)
    {
        Debug.Log($"[LevelManager] Level {e.LevelNumber} completed in {e.TimeRecord:F2} seconds. Success: {e.IsSuccess}");

        if (e.IsSuccess)
        {
            PlayerPrefs.SetInt(CurrentLevelKey, e.LevelNumber + 1);
            PlayerPrefs.SetFloat($"LevelTime_{e.LevelNumber}", e.TimeRecord);
            PlayerPrefs.Save();
            Debug.Log($"[LevelManager] Level {e.LevelNumber} marked as complete. Next level set to {e.LevelNumber + 1}");

            EventBus.Publish(new LevelEvents.LevelSuccessEvent());
            EventBus.Publish(new LevelEvents.LevelUnlockedEvent(e.LevelNumber + 1));
        }
        else
        {
            Debug.LogWarning($"[LevelManager] Level {e.LevelNumber} failed.");
            EventBus.Publish(new LevelEvents.LevelFailedEvent());
        }
    }
}
