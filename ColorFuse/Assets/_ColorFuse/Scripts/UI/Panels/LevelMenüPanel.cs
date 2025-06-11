using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LevelEvents;

public class LevelMenüPanel : Singleton<LevelMenüPanel> , IGameSystem
{
    [Header("References")]
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform contentParent;

    Dictionary<int, LevelButton> levelButtons = new Dictionary<int, LevelButton>();
    public void Initialize()
    {
        CreateLevelButtons();
        EventBus.Subscribe<LevelUnlockedEvent>(OnLevelUnlocked);
    }
    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelUnlockedEvent>(OnLevelUnlocked);
    }

    private void CreateLevelButtons()
    {

        Debug.Log($"Loaded levels count: {LevelManager.Instance.loadedLevels?.Count}");

        foreach (LevelConfig config in LevelManager.Instance.loadedLevels)
        {
            if (config.level <= 0)
            {
                Debug.LogWarning($"Skipping invalid level config with level number: {config.level}");
                continue;
            }
            GameObject buttonObj = Instantiate(levelButtonPrefab, contentParent);
            LevelButton levelButton = buttonObj.GetComponent<LevelButton>();

            LevelMetadata levelMetadata = new LevelMetadata(config, RuntimeDataManager.LoadRuntimeData(config.level));
            levelButton.Setup(levelMetadata);
            levelButtons[config.level] = levelButton;


        }
    }


    async Task OnLevelUnlocked(LevelUnlockedEvent e)
    {
        //objeyi buluyorum datayı güncelliyorum ve objeyi güncelliyorum burada
        LevelButton button = levelButtons[e.LevelNumber];
        button.UnlockedButton(true);
        
        
        await Task.CompletedTask;
    }

    
}
