using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelDatabase levelDatabase;
    int currentLevel;
    readonly string CurrentLevelKey = "CurrentLevel";
    readonly string TimeRecordKey = "TimeRecord";
    Dictionary<int, LevelConfig> LevelRecords;
    
    
    
}
