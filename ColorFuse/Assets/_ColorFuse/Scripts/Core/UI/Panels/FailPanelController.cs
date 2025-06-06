
using UnityEngine;
using UnityEngine.UI;

public class FailPanelController : Singleton<FailPanelController>, IGameSystem
{
    [SerializeField] private Button reStartButton;
    [SerializeField] private Button menuButton;


    public void Initialize()
    {
        reStartButton.onClick.AddListener(OnRestartButtonPressed);
        menuButton.onClick.AddListener(OnMenuButtonPressed);
    }
    public void Shutdown()
    {
        reStartButton.onClick.RemoveListener(OnRestartButtonPressed);
        menuButton.onClick.RemoveListener(OnMenuButtonPressed);
    }
    private void OnRestartButtonPressed()
    {
        Debug.Log("Restart button pressed FailPanelController");
        EventBus.Publish(new UIEvents.RestartButtonPressed(PlayerPrefs.GetInt(Constants.CurrentLevelKey)));
    }

    private void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed in WinPanel");
        EventBus.Publish(new UIEvents.ShowMainMenuEvent());
    }
}
