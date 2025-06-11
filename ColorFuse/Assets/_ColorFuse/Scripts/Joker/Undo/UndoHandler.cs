using UnityEngine;


public interface IUndoAction
{
    void Undo();
}

public class UndoHandler : MonoBehaviour, IGameSystem
{
    public void Initialize()
    {
        EventBus.Subscribe<UndoEvents.UndoRequestedEvent>(OnUndoRequested);
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<UndoEvents.UndoRequestedEvent>(OnUndoRequested);
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }

    private void OnLevelStarted(LevelEvents.LevelStartedEvent e)
    {
        EventBus.Publish(new UndoEvents.UndoUsageUpdatedEvent(true));
    }

    private void OnUndoRequested(UndoEvents.UndoRequestedEvent e)
    {
        if (!UndoManager.Instance.CanUndo())
        {
            Debug.Log("Undo hakkı kalmadı.");
            EventBus.Publish(new UndoEvents.UndoUsageUpdatedEvent(false));
            return;
        }

        bool result = UndoManager.Instance.Undo();
        if (result)
        {
            EventBus.Publish(new UndoEvents.UndoPerformedEvent(UndoManager.Instance.UndoCount));
            EventBus.Publish(new UndoEvents.UndoUsageUpdatedEvent(UndoManager.Instance.CanUndo()));
        }
    }
}

