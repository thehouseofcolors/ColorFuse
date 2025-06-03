
using UnityEngine;
using UnityEngine.UI;

public class FailPanelController : MonoBehaviour
{
    [SerializeField] private Button reStartButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        reStartButton.onClick.AddListener(OnRestartButtonPressed);
        menuButton.onClick.AddListener(OnMenuButtonPressed);
    }

    private void OnDestroy()
    {
        reStartButton.onClick.RemoveListener(OnRestartButtonPressed);
        menuButton.onClick.RemoveListener(OnMenuButtonPressed);
    }

    private void OnRestartButtonPressed()
    {
        Debug.Log("Next button pressed in WinPanel");
        EventBus.Publish(new LevelEvents.RestartButtonPressed());
    }

    private void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed in WinPanel");
        EventBus.Publish(new UIEvents.ShowMainMenuEvent());
    }
}
