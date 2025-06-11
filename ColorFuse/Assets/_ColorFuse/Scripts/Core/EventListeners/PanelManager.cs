using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using LevelEvents;
using UIEvents;

public enum UIPanelType
{
    MainMenu,
    LevelHUD,
    Success,
    Fail
}
public class PanelManager : MonoBehaviour, IGameSystem
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelHUDPanel;
    public GameObject winPanel;
    public GameObject failPanel;

    private List<GameObject> allPanels;

    void Start()
    {
        Debug.Log("[PanelManager] First scene set → MainMenu");

        // Panel listesi oluşturuluyor (start'ta bir kez)
        allPanels = new List<GameObject> { mainMenuPanel, levelHUDPanel, winPanel, failPanel };

        SetActivePanel(mainMenuPanel);
    }

    public void Initialize()
    {
        Debug.Log("[PanelManager] Initialized");
        EventBus.Subscribe<LevelLoadEvent>(OnLevelLoaded);
        EventBus.Subscribe<LevelButtonPressed>(OnLevelButtonPressed);
        EventBus.Subscribe<ShowPanelRequestEvent>(OnShowPanelRequest);

    }

    public void Shutdown()
    {
        Debug.Log("[PanelManager] Shutdown");
    }

    async Task OnLevelLoaded(LevelLoadEvent e)
    {
        Debug.Log("[PanelManager] LevelStarted → Showing HUD");
        await EventBus.PublishAsync(new ShowPanelRequestEvent(UIPanelType.LevelHUD));
    }

    async Task OnLevelButtonPressed(LevelButtonPressed e)
    {
        await EventBus.PublishAsync(new LevelLoadEvent(e.levelMetadata));
    }
    async Task OnShowPanelRequest(ShowPanelRequestEvent e)
    {
        GameObject panelToActivate = e.PanelType switch
        {
            UIPanelType.MainMenu => mainMenuPanel,
            UIPanelType.LevelHUD => levelHUDPanel,
            UIPanelType.Success => winPanel,
            UIPanelType.Fail => failPanel,
            _ => throw new System.NotImplementedException($"Panel type {e.PanelType} not handled")
        };

        SetActivePanel(panelToActivate);
        await Task.CompletedTask;
    }
    private void SetActivePanel(GameObject targetPanel)
    {
        foreach (var panel in allPanels)
        {
            panel.SetActive(panel == targetPanel);
        }

        Debug.Log($"[PanelManager] Active panel → {targetPanel.name}");
    }

}


// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.Events;
// using LevelEvents;
// using UIEvents;

// public enum UIPanelType
// {
//     MainMenu,
//     LevelHUD,
//     Success,
//     Fail
// }

// public class PanelManager : MonoBehaviour, IGameSystem
// {
//     [System.Serializable]
//     public class PanelConfiguration
//     {
//         public UIPanelType panelType;
//         public GameObject panelObject;
//         public UnityEvent onPanelActivated;
//     }

//     [Header("Panel Configuration")]
//     [SerializeField] private List<PanelConfiguration> panels = new List<PanelConfiguration>();

//     [Header("Transition Settings")]
//     [SerializeField] private float panelTransitionDelay = 0.2f;
//     [SerializeField] private bool debugLogs = true;

//     private Dictionary<UIPanelType, PanelConfiguration> panelDictionary;
//     private UIPanelType currentActivePanel;

//     private void Awake()
//     {
//         InitializePanelDictionary();
//     }

//     public void Initialize()
//     {
//         if (debugLogs) Debug.Log("[PanelManager] Initializing...");

//         EventBus.Subscribe<LevelLoadEvent>(OnLevelLoaded);
//         EventBus.Subscribe<LevelButtonPressed>(OnLevelButtonPressed);
//         EventBus.Subscribe<ShowPanelRequestEvent>(OnShowPanelRequest);

//         ShowDefaultPanel();
//     }

//     public void Shutdown()
//     {
//         if (debugLogs) Debug.Log("[PanelManager] Shutting down...");

//         EventBus.Unsubscribe<LevelLoadEvent>(OnLevelLoaded);
//         EventBus.Unsubscribe<LevelButtonPressed>(OnLevelButtonPressed);
//         EventBus.Unsubscribe<ShowPanelRequestEvent>(OnShowPanelRequest);
//     }

//     private void InitializePanelDictionary()
//     {
//         panelDictionary = new Dictionary<UIPanelType, PanelConfiguration>();

//         foreach (var config in panels)
//         {
//             if (config.panelObject == null)
//             {
//                 Debug.LogError($"Panel object for {config.panelType} is not assigned!");
//                 continue;
//             }

//             if (panelDictionary.ContainsKey(config.panelType))
//             {
//                 Debug.LogError($"Duplicate panel type found: {config.panelType}");
//                 continue;
//             }

//             panelDictionary.Add(config.panelType, config);
//             config.panelObject.SetActive(false);
//         }
//     }

//     private void ShowDefaultPanel()
//     {
//         if (debugLogs) Debug.Log("[PanelManager] Showing default panel → MainMenu");
//         RequestPanel(UIPanelType.MainMenu);
//     }

//     private async Task OnLevelLoaded(LevelLoadEvent e)
//     {
//         if (debugLogs) Debug.Log("[PanelManager] Level loaded → Showing HUD");
//         await RequestPanel(UIPanelType.LevelHUD);
//     }

//     private async Task OnLevelButtonPressed(LevelButtonPressed e)
//     {
//         await EventBus.PublishAsync(new LevelLoadEvent(e.levelMetadata));
//     }

//     private async Task OnShowPanelRequest(ShowPanelRequestEvent e)
//     {
//         await RequestPanel(e.PanelType);
//     }

//     public async Task RequestPanel(UIPanelType panelType)
//     {
//         if (currentActivePanel == panelType) return;

//         if (!panelDictionary.TryGetValue(panelType, out var panelConfig))
//         {
//             Debug.LogError($"Panel type {panelType} not found in configuration!");
//             return;
//         }

//         if (debugLogs) Debug.Log($"[PanelManager] Switching panel: {currentActivePanel} → {panelType}");

//         // Deactivate current panel
//         if (panelDictionary.TryGetValue(currentActivePanel, out var currentPanel))
//         {
//             currentPanel.panelObject.SetActive(false);
//         }

//         // Activate new panel
//         panelConfig.panelObject.SetActive(true);
//         panelConfig.onPanelActivated?.Invoke();
//         currentActivePanel = panelType;

//         // Small delay to allow UI to stabilize
//         await Task.Delay((int)(panelTransitionDelay * 1000));
//     }

//     // Editor-only validation
// #if UNITY_EDITOR
//     private void OnValidate()
//     {
//         // Ensure all enum values are covered
//         foreach (UIPanelType type in System.Enum.GetValues(typeof(UIPanelType)))
//         {
//             bool found = false;
//             foreach (var config in panels)
//             {
//                 if (config.panelType == type)
//                 {
//                     found = true;
//                     break;
//                 }
//             }

//             if (!found)
//             {
//                 Debug.LogWarning($"Panel type {type} is not configured in the PanelManager");
//             }
//         }
//     }
// #endif
// }



