

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHUDController : MonoBehaviour, IGameSystem
{
    [SerializeField] private Button shuffleButton;
    [SerializeField] private Button undoButton;
    [SerializeField] Button menüButton;
    [SerializeField] Button settingsButton;
    [SerializeField] private TextMeshProUGUI levelNumberText;

    private void OnEnable()
    {
        shuffleButton.onClick.AddListener(OnShufflePressed);
        undoButton.onClick.AddListener(OnUndoPressed);
        menüButton.onClick.AddListener(OnMenüPressed);
    }

    private void OnDestroy()
    {
        shuffleButton.onClick.RemoveListener(OnShufflePressed);
        undoButton.onClick.RemoveListener(OnUndoPressed);
        menüButton.onClick.RemoveListener(OnMenüPressed);
    }

    public void Initialize()
    {
        UpdateLevelNumber();
        EventBus.Subscribe<ShuffleEvents.ShuffleUsageUpdatedEvent>(OnShuffleStateChanged);
        EventBus.Subscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoStateChanged);
        EventBus.Subscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoUsageUpdated);

    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<ShuffleEvents.ShuffleUsageUpdatedEvent>(OnShuffleStateChanged);
        EventBus.Unsubscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoUsageUpdated);
        EventBus.Unsubscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoStateChanged);
    }
    private void UpdateLevelNumber()
    {
        levelNumberText.text = "Level " + PlayerPrefs.GetInt(LevelManager.CurrentLevelKey, 1);
    }
    public void SetInteractable(Button button, bool state)
    {
        button.interactable = state;
    }
    private void OnUndoUsageUpdated(UndoEvents.UndoUsageUpdatedEvent e)
    {
        undoButton.interactable = e.CanUndo;
    }


    

    private void OnShuffleStateChanged(ShuffleEvents.ShuffleUsageUpdatedEvent e)
    {
        SetInteractable(shuffleButton, e.CanShuffle);
    }

    private void OnUndoStateChanged(UndoEvents.UndoUsageUpdatedEvent e)
    {
        SetInteractable(undoButton, e.CanUndo);
    }

    private void OnShufflePressed()
    {
        Debug.Log("Shuffle button pressed");
        EventBus.Publish(new ShuffleEvents.ShuffleRequestedEvent());
    }

    private void OnUndoPressed()
    {
        Debug.Log("Undo button pressed");
        EventBus.Publish(new UndoEvents.UndoRequestedEvent());
    }
    void OnMenüPressed()
    {
        EventBus.Publish(new UIEvents.ShowMainMenuEvent());
    }
        
}

