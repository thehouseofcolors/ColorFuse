
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelController : Singleton<WinPanelController>, IGameSystem
{
    [SerializeField] private Button menuButton;


    public void Initialize()
    {
        menuButton.onClick.AddListener(OnMenuButtonPressed);
    }
    public void Shutdown()
    {
        menuButton.onClick.RemoveListener(OnMenuButtonPressed);
    }

    private void OnNextButtonPressed()
    {
        Debug.Log("Next button pressed in WinPanel");
    }

    private void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed in WinPanel");
    }
}
