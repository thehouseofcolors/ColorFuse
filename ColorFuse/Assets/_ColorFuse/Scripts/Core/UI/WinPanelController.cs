
using UnityEngine;
using UnityEngine.UI;

public class WinPanelController : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextButtonPressed);
        menuButton.onClick.AddListener(OnMenuButtonPressed);
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveListener(OnNextButtonPressed);
        menuButton.onClick.RemoveListener(OnMenuButtonPressed);
    }

    private void OnNextButtonPressed()
    {
        Debug.Log("Next button pressed in WinPanel");
        int nextLevel = PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1) + 1;
        PlayerPrefs.SetInt(LevelManager.CurrentLevelKey, nextLevel);
        PlayerPrefs.Save();
        
        EventBus.Publish(new LevelEvents.StartButtonPressed());
    }

    private void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed in WinPanel");
        EventBus.Publish(new UIEvents.ShowMainMenuEvent());
    }
}
