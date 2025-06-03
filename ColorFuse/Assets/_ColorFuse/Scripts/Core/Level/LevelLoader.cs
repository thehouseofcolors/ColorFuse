using UnityEngine;

public class LevelLoader : MonoBehaviour, IGameSystem
{
    [SerializeField] private LevelDatabase levelDatabase;

    private LevelConfig currentLevelConfig;

    public void Initialize()
    {
        EventBus.Subscribe<LevelEvents.StartButtonPressed>(OnLevelStarted);
        EventBus.Subscribe<LevelEvents.RestartButtonPressed>(OnLevelReplayed);
        Debug.Log("[LevelLoader] Initialized and subscribed to start/restart events.");
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelEvents.StartButtonPressed>(OnLevelStarted);
        EventBus.Unsubscribe<LevelEvents.RestartButtonPressed>(OnLevelReplayed);
        Debug.Log("[LevelLoader] Shutdown and unsubscribed from start/restart events.");
    }

    private void OnLevelStarted(LevelEvents.StartButtonPressed e)
    {
        int levelNum = PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1); PlayerPrefs.Save();
        Debug.Log($"[LevelLoader] OnLevelStarted received. Loading level {levelNum}.");

        currentLevelConfig = levelDatabase.GetLevelConfig(levelNum);
        if (currentLevelConfig.level == 0 && levelNum != 0)
        {
            Debug.LogWarning($"[LevelLoader] WARNING: Level {levelNum} config not found in LevelDatabase!");
        }
        else
        {
            Debug.Log($"[LevelLoader] LevelConfig found: Level={currentLevelConfig.level}, Time={currentLevelConfig.time}, ShuffleCount={currentLevelConfig.shuffleCount}");
        }

        EventBus.Publish(new LevelEvents.LevelStartedEvent(levelNum, false));
    }

    private void OnLevelReplayed(LevelEvents.RestartButtonPressed e)
    {
        int levelNum = PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1);
        Debug.Log($"[LevelLoader] OnLevelReplayed received. Reloading level {levelNum}.");

        currentLevelConfig = levelDatabase.GetLevelConfig(levelNum);
        if (currentLevelConfig.level == 0 && levelNum != 0)
        {
            Debug.LogWarning($"[LevelLoader] WARNING: Level {levelNum} config not found in LevelDatabase!");
        }
        else
        {
            Debug.Log($"[LevelLoader] LevelConfig found: Level={currentLevelConfig.level}, Time={currentLevelConfig.time}, ShuffleCount={currentLevelConfig.shuffleCount}");
        }

        EventBus.Publish(new LevelEvents.LevelStartedEvent(levelNum, true));
    }
}
