using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;

public class LevelMenuPanel : Singleton<LevelMenuPanel>
{
    [Header("References")]
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private RectTransform contentParent;

    Dictionary<int, LevelButton> levelButtons = new Dictionary<int, LevelButton>();



    public void CreateLevelButtons(List<LevelConfig> AllLevels)
    {

        foreach (LevelConfig config in AllLevels)
        {
            if (config.level <= 0)
            {
                Debug.LogWarning($"Skipping invalid level config with level number: {config.level}");
                continue;
            }
            GameObject buttonObj = Instantiate(levelButtonPrefab, contentParent);
            LevelButton levelButton = buttonObj.GetComponent<LevelButton>();
            levelButton.Setup(config);
            levelButtons[config.level] = levelButton;

        }
        levelButtons[1].UnlockeButton(true);
    }


    async Task OnLevelUnlocked(int level)
    {
        LevelButton button = levelButtons[level];
        button.UnlockeButton(true);
        
        
        await Task.CompletedTask;
    }

    
}
