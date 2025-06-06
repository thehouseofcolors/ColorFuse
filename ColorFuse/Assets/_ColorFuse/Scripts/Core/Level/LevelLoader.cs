using UnityEngine;

public class LevelLoader : MonoBehaviour, IGameSystem
{
    [SerializeField] LevelDatabase levelDatabase;
    public void Initialize()
    {
        EventBus.Subscribe<UIEvents.LevelButtonPressed>(OnLevelStarted);
        EventBus.Subscribe<UIEvents.NextButtonPressed>(OnNextStarted);
        EventBus.Subscribe<UIEvents.RestartButtonPressed>(OnLevelReplayed);
        Debug.Log("[LevelLoader] Initialized and subscribed to start/restart events.");
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<UIEvents.LevelButtonPressed>(OnLevelStarted);
        EventBus.Unsubscribe<UIEvents.NextButtonPressed>(OnNextStarted);
        EventBus.Unsubscribe<UIEvents.RestartButtonPressed>(OnLevelReplayed);
        Debug.Log("[LevelLoader] Shutdown and unsubscribed from start/restart events.");
    }

    private void OnLevelStarted(UIEvents.LevelButtonPressed e)
    {
        SetLevel(e.LevelConfig.level);

        EventBus.Publish(new LevelEvents.LevelStartedEvent(e.LevelConfig));
    }

    private void OnLevelReplayed(UIEvents.RestartButtonPressed e)
    {
        LevelConfig levelConfig =levelDatabase.GetLevelConfig(e.CurrentLevel);
        SetLevel(levelConfig.level);

        EventBus.Publish(new LevelEvents.LevelStartedEvent(levelConfig));
    }
    void OnNextStarted(UIEvents.NextButtonPressed e)
    {
        SetLevel(e.NextLevel);
        LevelConfig levelConfig =levelDatabase.GetLevelConfig(e.NextLevel);
        EventBus.Publish(new LevelEvents.LevelStartedEvent(levelConfig));
    }
    void SetLevel(int level)
    {
        PlayerPrefs.SetInt(Constants.CurrentLevelKey, level);
        PlayerPrefs.SetFloat(Constants.PlayStartTimeKey, Time.time);
        PlayerPrefs.Save();

    }


}
