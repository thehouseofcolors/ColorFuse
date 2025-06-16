using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LevelEvents;
using UIEvents;

public class LevelHUDController : Singleton<LevelHUDController>, IGameSystem
{
    [Header("UI References")]
    [SerializeField] private Button menuButton;
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject loadingIndicator;

    [Header("Settings")]
    [SerializeField] private float buttonCooldown = 0.5f;
    
    private bool _isInteractable = true;

    public void Initialize()
    {
        
        menuButton.onClick.AddListener(OnMenuPressedAsyncWrapper);

        UpdateLevelNumber();
        SetLoadingState(false);
    }

    public void Shutdown()
    {
        menuButton.onClick.RemoveListener(OnMenuPressedAsyncWrapper);
    }


    private void UpdateLevelNumber()
    {
        int currentLevel = PlayerPrefs.GetInt(Constants.CurrentLevelKey, 1);
        levelNumberText.text = $"LEVEL {currentLevel}";
    }

    public void SetInteractable(bool state)
    {
        _isInteractable = state;
        
        menuButton.interactable = state;
    }

    public void SetLoadingState(bool isLoading)
    {
        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(isLoading);
        }
        SetInteractable(!isLoading);
    }

    // Wrapper method since Unity events don't support async directly
    private async void OnMenuPressedAsyncWrapper() => await OnMenuPressed();

    private async Task OnMenuPressed()
    {
        if (!_isInteractable) return;

        try
        {
            SetInteractable(false);
            await EventBus.PublishAsync(new ShowPanelRequestEvent(UIPanelType.MainMenu));
            
            // Optional cooldown to prevent rapid clicking
            await Task.Delay((int)(buttonCooldown * 1000));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error handling menu button press: {e.Message}", this);
        }
        finally
        {
            SetInteractable(true);
        }
    }

}