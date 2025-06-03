

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button settings;
    [SerializeField] TextMeshProUGUI levelText;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonPressed);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartButtonPressed);
    }
    private void OnEnable()
    {
        UpdateLevelNumber();
    }

    private void UpdateLevelNumber()
    {
        levelText.text = "Level " + PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1);
    }
    private void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed in MainMenuPanel");
        EventBus.Publish(new LevelEvents.LevelStartedEvent());
    }
}

