using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct LevelButtonData
{
    public int levelNumber;
    public LevelButton levelButton;
    public GameObject button;
    public bool isLocked;

    public LevelButtonData(LevelButton levelButton, GameObject buttonObject)
    {
        this.levelButton = levelButton;
        button = buttonObject;

        if (levelButton != null)
        {
            levelNumber = levelButton.levelConfig.level;
            isLocked = levelButton.levelConfig.isLocked;
        }
        else
        {
            levelNumber = -1;
            isLocked = true;
            Debug.LogWarning("LevelButton or LevelConfig is null while creating LevelButtonData");
        }
    }

}

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    private int levelNumber;
    public LevelConfig levelConfig;
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
        PlayerPrefs.SetInt(Constants.CurrentLevelKey, levelNumber);
        PlayerPrefs.Save();

        Debug.Log($"Level {levelNumber} selected by button.");

        // Fire event to start the level
        EventBus.Publish(new UIEvents.LevelButtonPressed(levelConfig));
    }
    public void Unlock()
    {
        button.interactable = true;
    }

}
