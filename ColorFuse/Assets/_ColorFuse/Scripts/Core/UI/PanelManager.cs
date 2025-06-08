using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour, IGameSystem
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelHUDPanel;
    public GameObject winPanel;
    public GameObject failPanel;

    private UIAnimator mainAnimator;
    private UIAnimator hudAnimator;
    private UIAnimator winAnimator;
    private UIAnimator failAnimator;

    private void Awake()
    {
        mainAnimator = mainMenuPanel.GetComponent<UIAnimator>();
        hudAnimator = levelHUDPanel.GetComponent<UIAnimator>();
        winAnimator = winPanel.GetComponent<UIAnimator>();
        failAnimator = failPanel.GetComponent<UIAnimator>();
    }


    void Start()
    {
        Debug.Log("[PanelManager] First scene set → MainMenu");
        SetActivePanel(mainMenuPanel);
    }

    public void Initialize()
    {
        Debug.Log("[PanelManager] Initialized");
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<LevelEvents.LevelCompletedEvent>(OnLevelCompleted);
        EventBus.Subscribe<UIEvents.ShowMainMenuEvent>(OnShowMainMenu);
    }

    public void Shutdown()
    {
        Debug.Log("[PanelManager] Shutdown");
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<LevelEvents.LevelCompletedEvent>(OnLevelCompleted);
        EventBus.Unsubscribe<UIEvents.ShowMainMenuEvent>(OnShowMainMenu);
    }

    private void OnLevelStarted(LevelEvents.LevelStartedEvent e)
    {
        Debug.Log("[PanelManager] LevelStarted → Showing HUD");
        SetActivePanel(levelHUDPanel);
    }

    private void OnLevelCompleted(LevelEvents.LevelCompletedEvent e)
    {
        if (e.IsSuccess)
        {
            Debug.Log("[PanelManager] LevelCompleted (Success) → Showing WinPanel");
            SetActivePanel(winPanel);
            EventBus.Publish(new UIEvents.ShowSuccessPanelEvent());
        }
        else
        {
            Debug.Log("[PanelManager] LevelCompleted (Fail) → Showing FailPanel");
            SetActivePanel(failPanel);
            EventBus.Publish(new UIEvents.ShowFailPanelEvent());
        }
    }

    private void OnShowMainMenu(UIEvents.ShowMainMenuEvent e)
    {
        Debug.Log("[PanelManager] ShowMainMenuEvent → Showing MainMenu");
        SetActivePanel(mainMenuPanel);
    }

    // private void SetActivePanel(GameObject activePanel)
    // {
    //     Debug.Log("[PanelManager] Switching active panel to: " + (activePanel != null ? activePanel.name : "NULL"));

    //     mainMenuPanel?.SetActive(false);
    //     levelHUDPanel?.SetActive(false);
    //     winPanel?.SetActive(false);
    //     failPanel?.SetActive(false);

    //     if (activePanel != null)
    //         activePanel.SetActive(true);
    // }
    private Coroutine transitionCoroutine;

    private void SetActivePanel(GameObject targetPanel)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(SwitchPanelRoutine(targetPanel));
    }

    private IEnumerator SwitchPanelRoutine(GameObject targetPanel)
    {
        var animators = new[]
        {
            (mainMenuPanel, mainAnimator),
            (levelHUDPanel, hudAnimator),
            (winPanel, winAnimator),
            (failPanel, failAnimator)
        };

        foreach (var (panel, animator) in animators)
        {
            if (panel.activeSelf && panel != targetPanel)
            {
                yield return animator.FadeOut();
                panel.SetActive(false);
            }
        }

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);

            var animator = targetPanel.GetComponent<UIAnimator>();
            if (animator != null)
                yield return animator.FadeIn();
        }
    }

}
