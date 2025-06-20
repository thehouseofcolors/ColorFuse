using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;

public class UndoJokerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _earnButton;
    [SerializeField] private TextMeshProUGUI _countText;

    [Header("Settings")]
    [SerializeField] private int _adRewardAmount = 5;

    public void OnEnable()
    {
        _useButton.onClick.AddListener(OnUse);
        _earnButton.onClick.AddListener(OnEarn);
        UpdateUI();
    }

    public void OnDisable()
    {
        _useButton.onClick.RemoveAllListeners();
        _earnButton.onClick.RemoveAllListeners();

    }




    private async void OnUse()
    {
        if (PlayerPrefsService.RemainingUndo > 0)
        {
            PlayerPrefsService.RemainingUndo--;
            //undo yap
            UpdateUI();
        }
        await Task.CompletedTask;
    }

    private async void OnEarn()
    {
        _earnButton.interactable = false;

        bool rewarded = await AdManager.Instance.ShowRewardedAdAsync("undo_joker");
        if (rewarded)
        {
            PlayerPrefsService.RemainingUndo += _adRewardAmount;
            UpdateUI();
        }

        _earnButton.interactable = true;
    }

    private void UpdateUI()
    {
        int count = PlayerPrefsService.RemainingUndo;
        _countText.text = count.ToString();
        Debug.Log($"undo: {count}");
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        int count = PlayerPrefsService.RemainingUndo;
        _useButton.interactable = count > 0;
        _earnButton.interactable = count == 0;
    }
}

