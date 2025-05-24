using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    readonly string CurrentLevelKey = "CurrentLevel";
    readonly string TimeRecordKey = "TimeRecord";

    int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
        EventBus.Publish(new LevelStartedEvent(currentLevel));

    }

    public void CompleteLevel(int time, bool isSuccess)
    {
        // Kaydet
        PlayerPrefs.SetInt(TimeRecordKey + currentLevel, time);
        if (isSuccess)
            PlayerPrefs.SetInt(CurrentLevelKey, currentLevel + 1);
        PlayerPrefs.Save();

        // Event fırlat
        EventBus.Publish(new LevelCompletedEvent(currentLevel, time, isSuccess));
    }
}
