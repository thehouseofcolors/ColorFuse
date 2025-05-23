using UnityEngine;
using UnityEngine.UI;

public class RedistributeButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GridManager.Instance.RedistributeColors();
        Debug.Log("Renkler yeniden dağıtıldı!");
    }
}
