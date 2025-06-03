

using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class LevelProgressData
{
    public int levelNumber;
    public float timeRecord; // saniye cinsinden süre
}
[System.Serializable]
public class ProgressWrapper
{
    public List<LevelProgressData> levelProgressList = new List<LevelProgressData>();
}


public class ProgressManager : Singleton<ProgressManager>, IGameSystem
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "progress.json");

    public ProgressWrapper progressWrapper = new ProgressWrapper();

    void Awake()
    {
        LoadProgress();
    }
    public void Initialize() {}
    public void Shutdown() {}
    public void SaveLevelProgress(int level, float time)
    {
        var existing = progressWrapper.levelProgressList.Find(x => x.levelNumber == level);
        if (existing != null)
        {
            // Daha iyiyse güncelle
            if (time < existing.timeRecord)
                existing.timeRecord = time;
        }
        else
        {
            progressWrapper.levelProgressList.Add(new LevelProgressData
            {
                levelNumber = level,
                timeRecord = time
            });
        }

        SaveProgressToFile();
    }

    void SaveProgressToFile()
    {
        string json = JsonUtility.ToJson(progressWrapper, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("İlerleme kaydedildi:\n" + json);
    }

    void LoadProgress()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            progressWrapper = JsonUtility.FromJson<ProgressWrapper>(json);
            Debug.Log("İlerleme yüklendi.");
        }
        else
        {
            Debug.Log("İlerleme dosyası bulunamadı. Yeni kayıt oluşturulacak.");
            progressWrapper = new ProgressWrapper();
        }
    }

    public int GetLastUnlockedLevel()
    {
        int max = 1;
        foreach (var item in progressWrapper.levelProgressList)
        {
            if (item.levelNumber >= max)
                max = item.levelNumber + 1;
        }
        return max;
    }

    public float GetTimeForLevel(int level)
    {
        var item = progressWrapper.levelProgressList.Find(x => x.levelNumber == level);
        return item != null ? item.timeRecord : -1;
    }
}
