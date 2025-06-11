using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelConfigDataManager
{
    private static string StreamingPath => Path.Combine(Application.streamingAssetsPath, "levels.json");
    
    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> levels;
    }

    public static List<LevelConfig> LoadFromStreamingAssets()
    {
        Debug.Log("StreamingAssets path: " + StreamingPath);
        Debug.Log("File exists? " + File.Exists(StreamingPath));

        if (!File.Exists(StreamingPath))
        {
            Debug.LogWarning("StreamingAssets içinde level verisi bulunamadı.");
            return new List<LevelConfig>();
        }

        string json = File.ReadAllText(StreamingPath);
        Wrapper<LevelConfig> wrapper = JsonUtility.FromJson<Wrapper<LevelConfig>>(json);
        return wrapper?.levels ?? new List<LevelConfig>();
    }
}
