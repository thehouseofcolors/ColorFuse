using System.Collections.Generic;
using UnityEngine;

public class LevelProgressChecker
{
    private readonly List<Tile> _tiles;

    public LevelProgressChecker(List<Tile> tiles)
    {
        _tiles = tiles;
    }

    public void OnTileEmptied()
    {
        foreach (var tile in _tiles)
        {
            if (!tile.IsEmpty)
                return; // Seviye bitmedi
        }

        CompleteLevel();
    }

    private void CompleteLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(Constants.CurrentLevelKey);
        float time = PlayerPrefs.GetFloat(Constants.PlayStartTimeKey) - Time.time;
        bool isSuccess = true;

        Debug.Log("[LevelProgressChecker] Tüm tile'lar boş. Seviye tamamlandı.");
        EventBus.Publish(new LevelEvents.LevelCompletedEvent(currentLevel, time, isSuccess));
    }
}
