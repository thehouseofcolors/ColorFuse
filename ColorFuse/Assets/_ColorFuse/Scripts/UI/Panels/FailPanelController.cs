
using UnityEngine;
using UnityEngine.UI;

public class FailPanelController : Singleton<FailPanelController>, IGameSystem
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
    
    private void OnMenuButtonPressed()
    {
        Debug.Log("Menu button pressed in WinPanel");
    }
}
