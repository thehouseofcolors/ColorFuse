// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
// using GameEvents;
// using System;


// public class PanelManager : MonoBehaviour, IGameSystem
// {
//     [SerializeField] private GameObject loadingPanel;
//     [SerializeField] private GameObject menuPanel;
//     [SerializeField] private GameObject gamePanel;
//     [SerializeField] private GameObject winPanel;
//     [SerializeField] private GameObject fail_TimeOutPanel;
//     [SerializeField] private GameObject fail_NoMovePanel;

//     private GameObject currentPanel;
//     private IDisposable disposable;
//     private List<GameObject> allPanels;

//     public void Initialize()
//     {
//         allPanels = new List<GameObject> { loadingPanel, menuPanel, gamePanel, winPanel, fail_NoMovePanel,fail_TimeOutPanel };
//         disposable=EventBus.Subscribe<ScreenChangeEvent>(OnSceneChanged);
//     }

//     public void Shutdown()
//     {
//         disposable?.Dispose();
//     }

//     async Task OnSceneChanged(ScreenChangeEvent e)
//     {
//         await ShowPanelAsync(e.Screen);
//     }
//     public async Task ShowPanelAsync(ScreenType screenType)
//     {
//         var newPanel = GetPanel(screenType);

//         if (newPanel == null)
//         {
//             Debug.LogWarning($"[PanelManager] Unknown panel for screen type: {screenType}");
//             return;
//         }

//         foreach (var panel in allPanels)
//         {
//             if (panel == null) continue;

//             // Sadece farklıysa işlem yap
//             bool shouldBeActive = panel == newPanel;
//             if (panel.activeSelf != shouldBeActive)
//                 panel.SetActive(shouldBeActive);
//         }

//         currentPanel = newPanel;
//         await Task.CompletedTask;
//     }


//     private async Task HidePanelAsync(GameObject panel)
//     {
//         panel.SetActive(false);
//         await Task.CompletedTask;
//     }

//     private GameObject GetPanel(ScreenType type)
//     {
//         return type switch
//         {
//             ScreenType.Loading => loadingPanel,
//             ScreenType.Menu => menuPanel,
//             ScreenType.Game => gamePanel,
//             ScreenType.Win => winPanel,
//             ScreenType.Fail_NoMoves => fail_NoMovePanel,
//             ScreenType.Fail_TimeOver => fail_TimeOutPanel,
//             _ => null,
//         };
//     }
// }


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GameEvents;

public class PanelManager : MonoBehaviour, IGameSystem
{
    [Header("Panel References")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject fail_TimeOutPanel;
    [SerializeField] private GameObject fail_NoMovePanel;

    [Header("Settings")]
    [SerializeField] private float panelTransitionDelay = 0.2f;
    
    private GameObject _currentPanel;
    private IDisposable _eventSubscription;
    private Dictionary<ScreenType, GameObject> _panelDictionary;
    private bool _isTransitioning;

    public void Initialize()
    {
        CreatePanelDictionary();
        _eventSubscription = EventBus.Subscribe<ScreenChangeEvent>(OnScreenChanged);
    }

    public void Shutdown()
    {
        _eventSubscription?.Dispose();
    }

    private void CreatePanelDictionary()
    {
        _panelDictionary = new Dictionary<ScreenType, GameObject>
        {
            {ScreenType.Loading, loadingPanel},
            {ScreenType.Menu, menuPanel},
            {ScreenType.Game, gamePanel},
            {ScreenType.Win, winPanel},
            {ScreenType.Fail_NoMoves, fail_NoMovePanel},
            {ScreenType.Fail_TimeOver, fail_TimeOutPanel}
        };

        // Deactivate all panels initially
        foreach (var panel in _panelDictionary.Values)
        {
            if (panel != null) panel.SetActive(false);
        }
    }

    private async Task OnScreenChanged(ScreenChangeEvent e)
    {
        if (_isTransitioning) return;
        
        _isTransitioning = true;
        try
        {
            await ShowPanelAsync(e.Screen, e.TransitionData);
        }
        finally
        {
            _isTransitioning = false;
        }
    }

    public async Task ShowPanelAsync(ScreenType screenType, object transitionData = null)
    {
        if (!_panelDictionary.TryGetValue(screenType, out var newPanel))
        {
            Debug.LogError($"[PanelManager] No panel registered for {screenType}");
            return;
        }

        if (newPanel == null)
        {
            Debug.LogError($"[PanelManager] Panel reference for {screenType} is null");
            return;
        }

        // Only transition if it's a different panel
        if (newPanel != _currentPanel)
        {
            if (_currentPanel != null)
            {
                await HidePanelAsync(_currentPanel);
            }

            await ShowNewPanelAsync(newPanel, transitionData);
            _currentPanel = newPanel;
        }

        // Always send the panel shown event, even if it's the same panel
        await EventBus.PublishAsync(new PanelShownEvent(screenType, transitionData));
    }

    private async Task ShowNewPanelAsync(GameObject panel, object transitionData)
    {
        panel.SetActive(true);
        
        // Optional: Add animation/transition effects here
        if (panelTransitionDelay > 0)
        {
            await Task.Delay((int)(panelTransitionDelay * 1000));
        }
    }

    private async Task HidePanelAsync(GameObject panel)
    {
        // Optional: Add hide animation/transition effects here
        panel.SetActive(false);
        await Task.CompletedTask;
    }
}

// Add to your GameEvents namespace
public class PanelShownEvent : IGameEvent
{
    public ScreenType ScreenType { get; }
    public object TransitionData { get; }
    
    public PanelShownEvent(ScreenType screenType, object transitionData)
    {
        ScreenType = screenType;
        TransitionData = transitionData;
    }
}