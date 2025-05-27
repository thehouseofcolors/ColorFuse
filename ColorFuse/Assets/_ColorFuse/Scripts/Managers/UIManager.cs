using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelPanel;
    public GameObject winPanel;

    private void Start()
    {
        Debug.Log("UIManager started, showing Main Menu" + PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1) + "level");
        SetActivePanel(mainMenuPanel);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
    }

    private void OnLevelStarted(LevelStartedEvent evt)
    {
        Debug.Log($"LevelStartedEvent received for Level {evt.LevelNumber}");
        SetActivePanel(levelPanel);
    }

    private void OnLevelCompleted(LevelCompletedEvent evt)
    {
        Debug.Log($"LevelCompletedEvent received. Success:  Level: {evt.LevelNumber}");
        SetActivePanel(winPanel);
    }





    private void SetActivePanel(GameObject activePanel)
    {
        mainMenuPanel.SetActive(false);
        levelPanel.SetActive(false);
        winPanel.SetActive(false);

        if (activePanel != null)
            activePanel.SetActive(true);
    }

    // Button callbacks
    public void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed");
        EventBus.Publish(new LevelStartedEvent(PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1))); // 1. seviye örnek
    }

    public void OnRetryButtonPressed()
    {
        Debug.Log("Retry button pressed");
        EventBus.Publish(new LevelStartedEvent(LevelManager.Instance.currentLevel));
    }

    public void OnNextButtonPressed()
    {
        Debug.Log("Next button pressed");
        int nextLevel = LevelManager.Instance.currentLevel + 1;
        PlayerPrefs.SetInt(LevelManager.CurrentLevelKey, nextLevel);PlayerPrefs.Save();
        EventBus.Publish(new LevelStartedEvent(PlayerPrefs.GetInt(LevelManager.CurrentLevelKey)));
    }

    public void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed");
        SetActivePanel(mainMenuPanel);
    }

}
