using System.Collections.Generic;
using UnityEngine;


public class LevelManager : Singleton<LevelManager>
{
    public static string CurrentLevelKey = "CurrentLevel";
    // readonly string TimeRecordKey = "TimeRecord";

    public int currentLevel;
    private float levelStartTime;
    
    private bool levelCompleted;

    void Start()
    {
        Debug.Log("Level manager" + PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1) + "level");
        PlayerPrefs.DeleteAll();
        currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1); PlayerPrefs.Save();
        Debug.Log("manager" + PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1) + "level");
        levelStartTime = Time.time;
        levelCompleted = false;
    }


    
    private void OnEnable()
    {
        EventBus.Subscribe<LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
        EventBus.Subscribe<TileStateChangedEvent>(OnTileChanged);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
        EventBus.Unsubscribe<TileStateChangedEvent>(OnTileChanged);
    }
    void OnTileChanged(TileStateChangedEvent e)
    {
        if(GridManager.Instance.TilesCount == 0 && !levelCompleted)
        {
            levelCompleted = true;
            float timeSpent = Time.time - levelStartTime;
            EventBus.Publish(new LevelCompletedEvent(currentLevel, timeSpent));
        }
    }
    private void OnLevelStarted(LevelStartedEvent evt)
    {
        StartLevel(PlayerPrefs.GetInt(CurrentLevelKey, 1));
    }
    private void OnLevelCompleted(LevelCompletedEvent evt)
    {
        CompleteLevel(Time.time - levelStartTime);
    }

    
    void StartLevel(int level)
    {
        currentLevel = level;
        levelStartTime = Time.time;
        levelCompleted = false; 
    }

    void CompleteLevel(float time)
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        PlayerPrefs.SetFloat($"LevelTime_{currentLevel}", time);
        PlayerPrefs.Save();
    }
    
}
