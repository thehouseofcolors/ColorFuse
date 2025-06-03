using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour, IGameSystem
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelHUDPanel;
    public GameObject winPanel;
    public GameObject failPanel;


    void Start()
    {
        // EventBus.Publish(new LevelEvents.LevelStartedEvent(1, false));
        SetActivePanel(mainMenuPanel);
    }
    public void Initialize() {}
    public void Shutdown() {}
    
    private void OnEnable()
    {
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<LevelEvents.LevelSuccessEvent>(OnLevelCompleted);
        EventBus.Subscribe<LevelEvents.LevelFailedEvent>(OnLevelFailed);
        EventBus.Subscribe<UIEvents.ShowMainMenuEvent>(OnShowMainMenu);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<LevelEvents.LevelSuccessEvent>(OnLevelCompleted);
        EventBus.Unsubscribe<LevelEvents.LevelFailedEvent>(OnLevelFailed);
        EventBus.Unsubscribe<UIEvents.ShowMainMenuEvent>(OnShowMainMenu);
    }

    private void OnLevelStarted(LevelEvents.LevelStartedEvent e)
    {
        SetActivePanel(levelHUDPanel);
    }

    private void OnLevelCompleted(LevelEvents.LevelSuccessEvent e)
    {
        SetActivePanel(winPanel);
        
    }

    private void OnLevelFailed(LevelEvents.LevelFailedEvent e)
    {
        SetActivePanel(failPanel);
    }

    private void OnShowMainMenu(UIEvents.ShowMainMenuEvent e)
    {
        SetActivePanel(mainMenuPanel);
    }

    private void SetActivePanel(GameObject activePanel)
    {
        mainMenuPanel.SetActive(false);
        levelHUDPanel.SetActive(false);
        winPanel.SetActive(false);
        failPanel?.SetActive(false);

        if (activePanel != null)
            activePanel.SetActive(true);
    }
    
    
}
