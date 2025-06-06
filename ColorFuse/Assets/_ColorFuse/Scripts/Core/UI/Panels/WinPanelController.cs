
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelController : Singleton<WinPanelController>, IGameSystem
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button menuButton;


    public void Initialize()
    {
        nextButton.onClick.AddListener(OnNextButtonPressed);
        menuButton.onClick.AddListener(OnMenuButtonPressed);
    }
    public void Shutdown()
    {
        nextButton.onClick.RemoveListener(OnNextButtonPressed);
        menuButton.onClick.RemoveListener(OnMenuButtonPressed);
    }

    private void OnNextButtonPressed()
    {
        Debug.Log("Next button pressed in WinPanel");
        EventBus.Publish(new UIEvents.NextButtonPressed(PlayerPrefs.GetInt(Constants.CurrentLevelKey)));
    }

    private void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed in WinPanel");
        EventBus.Publish(new UIEvents.ShowMainMenuEvent());
    }
}
