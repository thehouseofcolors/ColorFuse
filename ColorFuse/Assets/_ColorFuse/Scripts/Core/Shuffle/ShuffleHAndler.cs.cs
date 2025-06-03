using UnityEngine;

public class ShuffleHandler : MonoBehaviour, IGameSystem
{
    [SerializeField] private LevelDatabase levelDatabase;
    [SerializeField] private int maxShuffleUses;

    private int remainingShuffles;

    public void Initialize()
    {
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<ShuffleEvents.ShuffleRequestedEvent>(OnShuffleRequested);
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<ShuffleEvents.ShuffleRequestedEvent>(OnShuffleRequested);
    }

    private void OnLevelStarted(LevelEvents.LevelStartedEvent e)
    {
        if (levelDatabase == null)
        {
            Debug.LogWarning("[ShuffleHandler] LevelDatabase is not assigned!");
            return;
        }

        var levelConfig = levelDatabase.GetLevelConfig(e.Level);
       
        maxShuffleUses = Mathf.Max(0, levelConfig.shuffleCount);
        remainingShuffles = maxShuffleUses;

        Debug.Log($"[ShuffleHandler] Level {e.Level} started. Max shuffle uses: {maxShuffleUses}");
    }

    private void OnShuffleRequested(ShuffleEvents.ShuffleRequestedEvent e)
    {
        if (remainingShuffles <= 0)
        {
            Debug.LogWarning("[ShuffleHandler] No remaining shuffles left.");
            return;
        }

        remainingShuffles--;

        Debug.Log($"[ShuffleHandler] Shuffle requested. Remaining shuffles: {remainingShuffles}");

        EventBus.Publish(new ShuffleEvents.ShuffledEvent(remainingShuffles));
        EventBus.Publish(new ShuffleEvents.ShuffleUsageUpdatedEvent(remainingShuffles <= 0));
    }
}
