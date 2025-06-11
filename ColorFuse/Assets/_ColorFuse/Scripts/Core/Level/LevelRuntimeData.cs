using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelRuntimeData
{
    public float RemainingTime;
    public int RemainingUndo;
    public int RemainingShuffle;
    public bool IsLevelUnlocked;
    public bool IsSuccess;

    public LevelRuntimeData(bool isUnlocked = false, float time = 60f, int undo = 3, int shuffle = 3)
    {
        RemainingTime = time;
        RemainingUndo = undo;
        RemainingShuffle = shuffle;
        IsLevelUnlocked = isUnlocked;
        IsSuccess = false;
    }
}


public static class RuntimeDataManager
{
    private static string GetRuntimeDataPath(int level)
    {
        return Path.Combine(Application.persistentDataPath, $"level_runtime_{level}.json");
    }

    public static void SaveRuntimeData(int level, LevelRuntimeData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetRuntimeDataPath(level), json);
        Debug.Log($"Runtime data saved for level {level}");
    }

    public static LevelRuntimeData? LoadRuntimeData(int level)
    {
        Debug.Log("Dosya yolu: " + Application.persistentDataPath);

        string path = GetRuntimeDataPath(level);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<LevelRuntimeData>(json);
        }
        else
        {
            Debug.LogWarning($"Runtime data not found for level {level}");
            return null;
        }
    }
}
