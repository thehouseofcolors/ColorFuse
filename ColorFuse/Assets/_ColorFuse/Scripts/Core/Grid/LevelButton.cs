using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UIEvents;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI levelText;

    // [SerializeField] private GameObject lockedIcon;
    // [SerializeField] private GameObject completedIcon;


    private Button _button;
    private int _levelNumber;
    private LevelMetadata _levelMetadata;
    private LevelRuntimeData _levelRuntimeData;
    private LevelConfig _levelConfig;
 

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickAsyncWrapper);
    }

    public void Setup(LevelMetadata metadata)
    {
        if (metadata == null)
        {
            Debug.LogError("LevelMetadata is null!", this);
            return;
        }

        _levelNumber = metadata.levelNumber;
        _levelMetadata = metadata;
        UpdateVisualState();
    }

    // Wrapper method to handle async onClick since Unity events don't support async directly
    private async void OnClickAsyncWrapper()
    {
        await OnClickAsync();
    }

    private async Task OnClickAsync()
    {
        if (!_button.interactable) return;

        try
        {
            PlayerPrefsService.CurrentLevel = _levelNumber;
            Debug.Log("levelButton click");
            await EventBus.PublishAsync(new LevelButtonPressed(_levelMetadata));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error handling level button click: {e.Message}", this);
            // Consider adding visual feedback for the error state
        }
    }

    public void UnlockedButton(bool isUnlocked)
    {
        _button.interactable = isUnlocked;
        
        _levelRuntimeData.IsLevelUnlocked = true;


        // if (lockedIcon != null)
        //     lockedIcon.SetActive(isLocked);

        UpdateVisualState();
    }

    public void SetCompletedState(bool isCompleted)
    {
        // if (completedIcon != null)
        //     completedIcon.SetActive(isCompleted);

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