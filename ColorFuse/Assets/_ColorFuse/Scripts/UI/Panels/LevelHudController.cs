using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LevelEvents;
using UIEvents;

public class LevelHUDController : Singleton<LevelHUDController>, IGameSystem
{
    [Header("UI References")]
    [SerializeField] private Button shuffleButton;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject loadingIndicator;

    [Header("Settings")]
    [SerializeField] private float buttonCooldown = 0.5f;
    
    private bool _isInteractable = true;

    public void Initialize()
    {
        ValidateReferences();
        
        menuButton.onClick.AddListener(OnMenuPressedAsyncWrapper);
        settingsButton.onClick.AddListener(OnSettingsPressed);
        shuffleButton.onClick.AddListener(OnShufflePressed);
        undoButton.onClick.AddListener(OnUndoPressed);

        UpdateLevelNumber();
        SetLoadingState(false);
    }

    public void Shutdown()
    {
        menuButton.onClick.RemoveListener(OnMenuPressedAsyncWrapper);
        settingsButton.onClick.RemoveListener(OnSettingsPressed);
        shuffleButton.onClick.RemoveListener(OnShufflePressed);
        undoButton.onClick.RemoveListener(OnUndoPressed);
    }

    private void ValidateReferences()
    {
        Debug.Assert(menuButton != null, "Menu button reference is missing!", this);
        Debug.Assert(levelNumberText != null, "Level number text reference is missing!", this);
        // Add other assertions as needed
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
        settingsButton.interactable = state;
        shuffleButton.interactable = state;
        undoButton.interactable = state;
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

    private void OnSettingsPressed()
    {
        if (!_isInteractable) return;
        // Implement settings logic
    }

    private void OnShufflePressed()
    {
        if (!_isInteractable) return;
        // Implement shuffle logic
    }

    private void OnUndoPressed()
    {
        if (!_isInteractable) return;
        // Implement undo logic
    }
}