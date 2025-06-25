using System.Threading.Tasks;
using UnityEngine;
using GameEvents;
using System.Collections.Generic;
using System.Linq;

using System;



#region State Implementations

public class MenuState : IGameState
{
    public async Task EnterAsync()
    {
        Debug.Log("[State] Entering Menu");

        await EventBus.PublishAsync(new ScreenChangeEvent(ScreenType.Menu));

        // Menüde bir sonraki levelı preload et (opsiyonel, GameFlowController zaten yapıyor)
        // await LevelManager.Instance.PreloadLevelAsync(PlayerPrefsService.CurrentLevel);
    }

    public Task ExitAsync() => Task.CompletedTask;
}


public class GamePlayState : IGameState
{
    private readonly LevelConfig _levelConfig;
    private IDisposable _gameEndSubscription;

    public GamePlayState(LevelConfig levelConfig) => _levelConfig = levelConfig;

    public async Task EnterAsync()
    {
        Debug.Log($"[State] Starting Level {_levelConfig.level}");
        if (_levelConfig == null)
        {
            Debug.LogError("LevelConfig is NULL in GamePlayState!");
        }
        else
        {
            Debug.Log($"LevelConfig level is {_levelConfig.level}");
        }

        await EventBus.PublishAsync(new ScreenChangeEvent(ScreenType.Game));
        await EventBus.PublishAsync(new GameLoadEvent(_levelConfig));
        // await EventBus.PublishAsync(new AudioEvent(AudioType.Music, "game_music", true));

        GameTimer.Instance.StartTimer(_levelConfig.timeLimit);

        // Subscribe to game end conditions
        _gameEndSubscription = EventBus.Subscribe<GameEndConditionMetEvent>(OnGameEndCondition);
        
    }

    public async Task ExitAsync()
    {
        _gameEndSubscription?.Dispose();
        GameTimer.Instance.StopTimer();
        // await EventBus.PublishAsync(new AudioEvent(AudioType.SFX, "level_exit"));
        await Task.CompletedTask;
    }
    private async void OnGameEndCondition(GameEndConditionMetEvent e)
    {
        if (e.IsWin)
        {
            await GameStateMachine.ChangeStateAsync(new GameWinState());
        }
        else
        {
            await GameStateMachine.ChangeStateAsync(new GameFailState(e.FailReason));
        }
    }

   
}

public class GamePauseState : IGameState
{
    public GamePauseType gamePauseType;
    public async Task EnterAsync()
    {
        Debug.Log("[State] Game Paused");
        GameTimer.Instance.Pause();
        await EventBus.PublishAsync(new GamePauseEvent(gamePauseType));
    }

    public async Task ExitAsync()
    {
        Debug.Log("Game resumed");
        GameTimer.Instance.Resume();
        await Task.CompletedTask;
    }
}

public class GameWinState : IGameState
{
    public async Task EnterAsync()
    {
        Debug.Log("[State] Level Completed!");

        GameTimer.Instance.StopTimer();
        PlayerPrefsService.IncrementLevel();

        await EventBus.PublishAsync(new GameWinEvent());
        await EventBus.PublishAsync(new ScreenChangeEvent(ScreenType.Win));
        // await EventBus.PublishAsync(new AudioEvent(AudioType.SFX, "level_win"));

        // Auto-proceed to next level after delay
        await Task.Delay(2000);

        // await GameStateMachine.ChangeStateAsync(new GamePlayState(LevelManager.GetNextLevelConfig()));//bunu button ile yapcam
    }

    public Task ExitAsync() => Task.CompletedTask;
}
public class GameFailState : IGameState
{
    private readonly GameFailType _reason;

    public GameFailState(GameFailType reason) => _reason = reason;

    public async Task EnterAsync()
    {
        Debug.Log($"[State] Level Failed: {_reason}");

        // await EventBus.PublishAsync(new GameFailEvent(_reason));

        ScreenType screenType = _reason switch
        {
            GameFailType.TimeOver => ScreenType.Fail_TimeOver,
            GameFailType.NoMoves => ScreenType.Fail_NoMoves,
            _ => ScreenType.Menu
        };

        await EventBus.PublishAsync(new ScreenChangeEvent(screenType));
    }

    public Task ExitAsync() => Task.CompletedTask;
}


#endregion

#region State Helpers

public static class GameStateHelper
{

    
    public static async Task ReturnToMenu() => 
        await GameStateMachine.ChangeStateAsync(new MenuState());
}

#endregion


