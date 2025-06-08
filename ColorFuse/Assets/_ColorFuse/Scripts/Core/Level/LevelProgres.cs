using System.Collections;
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
        GridManager gridManager = GameObject.FindFirstObjectByType<GridManager>();
        if (gridManager != null)
        {
            gridManager.StartCoroutine(CompleteLevelRoutine(gridManager));
        }
        else
        {
            Debug.LogError("[LevelProgressChecker] GridManager bulunamadı!");
        }
    }

    private IEnumerator CompleteLevelRoutine(GridManager gridManager)
    {
        yield return gridManager.StartCoroutine(gridManager.ClearGridAnimated());

        int currentLevel = PlayerPrefs.GetInt(Constants.CurrentLevelKey);
        float time = PlayerPrefs.GetFloat(Constants.PlayStartTimeKey) - Time.time;
        bool isSuccess = true;

        Debug.Log("[LevelProgressChecker] Seviye başarıyla tamamlandı.");
        EventBus.Publish(new LevelEvents.LevelCompletedEvent(currentLevel, time, isSuccess));
    }

}
