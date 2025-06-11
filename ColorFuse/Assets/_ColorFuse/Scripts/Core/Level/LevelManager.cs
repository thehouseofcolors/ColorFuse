using System.Collections.Generic;
using UnityEngine;
using LevelEvents;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using System.Threading.Tasks;

[System.Serializable]
public class LevelMetadata
{
    public int levelNumber;
    public LevelConfig levelConfig;
    public LevelRuntimeData levelRuntimeData;

    public LevelMetadata(LevelConfig levelConfig, LevelRuntimeData levelRuntimeData)
    {
        this.levelConfig = levelConfig;
        this.levelRuntimeData = levelRuntimeData;
        levelNumber = levelConfig.level;
    }

}



[System.Serializable]
public class LevelConfig
{
    public int level;
    public int gridWidth;
    public int gridLength;
    public int whites;
    public string seed;
}

public enum PlayStatus{Started, Completed, NotStarted}

public class LevelManager : Singleton<LevelManager>, IGameSystem
{
    public List<LevelConfig> loadedLevels = new List<LevelConfig>();

    HashSet<int> unlockedLevels = new HashSet<int>();
    void Awake()
    {
        // Json'dan verileri yükle
        loadedLevels = LevelConfigDataManager.LoadFromStreamingAssets();

        // Veya boşsa default veri oluştur
        if (loadedLevels.Count == 0)
        {
            Debug.Log("Hiç level bulunamadı, default level yükleniyor...");
            // default oluştur ya da ScriptableObject'ten yükle vs.
        }
    }
    public void Initialize()
    {
        EventBus.Subscribe<LevelUnlockedEvent>(OnLevelUnlocked);

    }
    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelUnlockedEvent>(OnLevelUnlocked);

    }
    public async Task OnLevelUnlocked(LevelUnlockedEvent e)
    {
        unlockedLevels.Add(e.LevelNumber);

        await Task.CompletedTask;

    }
    public bool IsUnlocked(int level)
    {
        return unlockedLevels.Contains(level);
    }
}
