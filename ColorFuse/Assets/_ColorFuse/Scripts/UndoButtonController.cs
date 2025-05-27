using UnityEngine;
using UnityEngine.UI;

public class UndoButtonController : MonoBehaviour
{
    public Button undoButton;

    private void Start()
    {
        undoButton.onClick.AddListener(OnUndoClicked);
    }

    private void OnUndoClicked()
    {
        
    }

    public void SetInteractable(bool state)
    {
        undoButton.interactable = state;
    }
}
