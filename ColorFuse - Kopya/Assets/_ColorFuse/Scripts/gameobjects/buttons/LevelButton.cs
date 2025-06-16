using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using GameEvents;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI levelText;

    public bool isLocked;
    private Button _button;
    private int _levelNumber;
    private LevelConfig _levelConfig;
    
    private bool _isClicked = false;

    public void Setup(LevelConfig level)
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickAsyncWrapper);
        if (level == null)
        {
            Debug.LogError("Level is null!", this);
            return;
        }
        _levelConfig = level;
        _levelNumber = _levelConfig.level;
        UnlockeButton(false);
        UpdateVisualState();
    }
    private async Task OnClickAsync()
    {
        if (!_button.interactable || _isClicked)
        {
            Debug.LogWarning("Button blocked due to isClicked or not interactable.");
            return;
        }

        _isClicked = true;
        Debug.LogWarning("LEVEL BUTTON: Click accepted.");

        try
        {
            PlayerPrefsService.CurrentLevel = _levelNumber;
            await GameStateMachine.ChangeStateAsync(new GamePlayState(_levelConfig));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e}");
        }
        finally
        {
            _isClicked = false; // Bu kısmı şimdilik YORUMA al
        }
    }


    // Wrapper method to handle async onClick since Unity events don't support async directly
    private async void OnClickAsyncWrapper()
    {
        await OnClickAsync();
    }

    
    public void UnlockeButton(bool isUnlocked)
    {
        _button.interactable = isUnlocked;


        // if (lockedIcon != null)
        //     lockedIcon.SetActive(isLocked);

        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (levelText != null)
            levelText.text = _levelNumber.ToString();


    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(OnClickAsyncWrapper);
        
        // RuntimeDataManager.SaveRuntimeData(_levelNumber, _levelRuntimeData);
    }

}