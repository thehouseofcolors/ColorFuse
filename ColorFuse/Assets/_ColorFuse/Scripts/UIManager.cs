using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelPanel;
    public GameObject winPanel;
    public GameObject failPanel;

    private void Start()
    {
        Debug.Log("UIManager started, showing Main Menu");
        ShowMainMenu();
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
        ShowLevelPanel();
    }

    private void OnLevelCompleted(LevelCompletedEvent evt)
    {
        Debug.Log($"LevelCompletedEvent received. Success: {evt.isSccessfull}, Level: {evt.LevelNumber}");
        if (evt.isSccessfull)
            ShowWinPanel();
        else
            ShowFailPanel();
    }

    public void ShowMainMenu()
    {
        Debug.Log("Showing Main Menu");
        SetActivePanel(mainMenuPanel);
    }

    public void ShowLevelPanel()
    {
        Debug.Log("Showing Level Panel");
        SetActivePanel(levelPanel);
    }

    public void ShowWinPanel()
    {
        Debug.Log("Showing Win Panel");
        SetActivePanel(winPanel);
    }

    public void ShowFailPanel()
    {
        Debug.Log("Showing Fail Panel");
        SetActivePanel(failPanel);
    }

    private void SetActivePanel(GameObject activePanel)
    {
        mainMenuPanel.SetActive(false);
        levelPanel.SetActive(false);
        winPanel.SetActive(false);
        failPanel.SetActive(false);

        if (activePanel != null)
            activePanel.SetActive(true);
    }

    // Button callbacks
    public void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed");
        EventBus.Publish(new LevelStartedEvent(1)); // 1. seviye örnek
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
        EventBus.Publish(new LevelStartedEvent(nextLevel));
    }

    public void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed");
        ShowMainMenu();
    }
}
