// UndoButton.cs
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => UndoManager.Instance.Undo());
    }
}
