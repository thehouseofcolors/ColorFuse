using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class LevelMenüPanel : Singleton<LevelMenüPanel> , IGameSystem
{
    [Header("References")]
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private LevelDatabase levelDatabase;
    List<LevelButtonData> levels = new List<LevelButtonData>();
  
    public void Initialize()
    {
        CreateLevelButtons();
        EventBus.Subscribe<LevelEvents.LevelUnlockedEvent>(OnLevelUnlocked);
    }
    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelEvents.LevelUnlockedEvent>(OnLevelUnlocked);
    }

    private bool ValidateFields()
    {
        return levelButtonPrefab != null && contentParent != null && levelDatabase != null;
    }

    private void CreateLevelButtons()
    {
        if (levelDatabase.configs == null || levelDatabase.configs.Count == 0)
        {
            Debug.LogWarning("LevelSelectManager: No level configs found in database.");
            return;
        }

        foreach (LevelConfig config in levelDatabase.configs)
        {
            if (config.level <= 0)
            {
                Debug.LogWarning($"Skipping invalid level config with level number: {config.level}");
                continue;
            }

            GameObject buttonObj = Instantiate(levelButtonPrefab, contentParent);
            LevelButton levelButton = buttonObj.GetComponent<LevelButton>();

            if (levelButton != null)
            {
                levelButton.Setup(config);
            }
            else
            {
                Debug.LogError("LevelButton component missing on prefab.");
            }

            levels.Add(new LevelButtonData(levelButton, buttonObj));
            

        }
    }


    void OnLevelUnlocked(LevelEvents.LevelUnlockedEvent e)
    {
        var level = levels.FirstOrDefault(l => l.levelNumber == e.LevelNumber);
        level.levelButton.Unlock();

    }

    
}
