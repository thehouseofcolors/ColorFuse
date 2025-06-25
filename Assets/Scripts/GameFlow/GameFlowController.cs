using System;
using System.Threading.Tasks;
using UnityEngine;
using GameEvents;
using System.Collections.Generic;

public class GameFlowController : MonoBehaviour, IGameSystem
{
    private List<IDisposable> _subscriptions = new();
 
    public void Initialize()
    {
        _subscriptions.Add(EventBus.Subscribe<GameStartRequestedEvent>(OnGameStart));
        _subscriptions.Add(EventBus.Subscribe<NextLevelRequestedEvent>(OnNextLevelRequested));
        _subscriptions.Add(EventBus.Subscribe<LevelRestartRequestedEvent>(OnLevelRestartRequested));
        _subscriptions.Add(EventBus.Subscribe<MenuRequestedEvent>(OnMenuRequested));

        // Menüde preload yap
    }
    public void Shutdown()
    {
        foreach (var sub in _subscriptions)
            sub.Dispose();
        _subscriptions.Clear();

    }

    private async Task OnGameStart(GameStartRequestedEvent e)
    {


    }

    private async Task OnNextLevelRequested(NextLevelRequestedEvent e)
    {
        PlayerPrefsService.IncrementLevel();
        await OnGameStart(new GameStartRequestedEvent());
    }

    private async Task OnLevelRestartRequested(LevelRestartRequestedEvent e)
    {
        await OnGameStart(new GameStartRequestedEvent());
    }

    private async Task OnMenuRequested(MenuRequestedEvent e)
    {
        await GameStateMachine.ChangeStateAsync(new MenuState());
    }
}
