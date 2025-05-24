

using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
    }

    private void OnLevelCompleted(LevelCompletedEvent evt)
    {
        Debug.Log($"LEVEL UI - Tamamlanan Seviye: {evt.TimeRecord}");
        // Burada UI güncellenebilir
    }
}
