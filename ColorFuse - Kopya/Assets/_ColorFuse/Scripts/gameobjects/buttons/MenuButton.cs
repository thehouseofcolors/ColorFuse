using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;

public class MenuButton : MonoBehaviour
{
    [SerializeField] Button menuButton;


    public void OnEnable()
    {
        menuButton.onClick.AddListener(OnUse);
    }

    public void OnDisable()
    {
        menuButton.onClick.RemoveAllListeners();
    }
    

    private async void OnUse()
    {
        await AdManager.Instance.ShowInterstitialAdAsync();//bazen reklam olabilir bence
        await GameStateMachine.ChangeStateAsync(new MenuState());
    }


}
