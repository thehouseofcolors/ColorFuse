

using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class LevelProgressData
{
    public int seed;
    public int levelNumber;
    public float maxTimeAvaible;
    public bool isLocked;
    public bool isStarted;
}
[System.Serializable]
public class ProgressWrapper
{
    public int totalLevelCount = 99;
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

}
