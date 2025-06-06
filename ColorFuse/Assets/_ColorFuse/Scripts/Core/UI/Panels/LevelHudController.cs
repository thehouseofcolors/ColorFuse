

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHUDController : Singleton<LevelHUDController>, IGameSystem
{
    [SerializeField] private Button shuffleButton;
    [SerializeField] private Button undoButton;
    [SerializeField] Button menüButton;
    [SerializeField] Button settingsButton;
    [SerializeField] private TextMeshProUGUI levelNumberText;

    public void Initialize()
    {
        shuffleButton.onClick.AddListener(OnShufflePressed);
        undoButton.onClick.AddListener(OnUndoPressed);
        menüButton.onClick.AddListener(OnMenüPressed);

        UpdateLevelNumber();

        EventBus.Subscribe<ShuffleEvents.ShuffleUsageUpdatedEvent>(OnShuffleStateChanged);
        EventBus.Subscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoUsageUpdated);
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }

    public void Shutdown()
    {
        shuffleButton.onClick.RemoveListener(OnShufflePressed);
        undoButton.onClick.RemoveListener(OnUndoPressed);
        menüButton.onClick.RemoveListener(OnMenüPressed);

        EventBus.Unsubscribe<ShuffleEvents.ShuffleUsageUpdatedEvent>(OnShuffleStateChanged);
        EventBus.Unsubscribe<UndoEvents.UndoUsageUpdatedEvent>(OnUndoUsageUpdated);
        
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }

    private void UpdateLevelNumber()
    {
        levelNumberText.text = "Level " + PlayerPrefs.GetInt(Constants.CurrentLevelKey, 1);
    }
    public void SetInteractable(Button button, bool state)
    {
        button.interactable = state;
    }
    private void OnUndoUsageUpdated(UndoEvents.UndoUsageUpdatedEvent e)
    {
        undoButton.interactable = e.CanUndo;

        SetInteractable(undoButton, e.CanUndo);
    }




    private void OnShuffleStateChanged(ShuffleEvents.ShuffleUsageUpdatedEvent e)
    {
        SetInteractable(shuffleButton, e.CanShuffle);
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
    void OnLevelStarted(LevelEvents.LevelStartedEvent e)
    {
        UpdateLevelNumber();
    }
        
}

