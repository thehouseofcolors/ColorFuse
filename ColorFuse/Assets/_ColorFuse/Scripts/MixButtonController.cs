

using UnityEngine;
using UnityEngine.UI;

public class MixButtonController : MonoBehaviour
{
    public Button mixButton;

    private void Start()
    {
        mixButton.onClick.AddListener(OnMixClicked);
    }

    private void OnMixClicked()
    {
        
    }

    public void SetInteractable(bool state)
    {
        mixButton.interactable = state;
    }
}
