using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    private int levelNumber;
    private LevelConfig levelConfig;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError($"LevelButton is missing a Button component on {gameObject.name}");
            return;
        }

        button.onClick.AddListener(OnClick);
    }

    public void Setup(LevelConfig config)
    {
        levelNumber = config.level;
        levelConfig = config;

        if (levelText != null)
        {
            levelText.text = levelNumber.ToString();
        }
        else
        {
            Debug.LogWarning($"LevelText not assigned in {gameObject.name}");
        }

        if (button != null)
        {
            button.interactable = !levelConfig.isLocked;
        }
    }

    private void OnClick()
    {
        if (levelNumber < 1)
        {
            Debug.LogError("Invalid level number clicked!");
            return;
        }

        // Save current level
        PlayerPrefs.SetInt(LevelManager.CurrentLevelKey, levelNumber);
        PlayerPrefs.Save();

        Debug.Log($"Level {levelNumber} selected by button.");

        // Fire event to start the level
        EventBus.Publish(new LevelEvents.LevelStartedEvent(levelNumber));
    }
}
