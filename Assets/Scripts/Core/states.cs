using System.Threading.Tasks;
using UnityEngine;
using GameEvents;
using System.Collections.Generic;
using System.Linq;


public class MenuState : IGameState
{
    public async Task EnterAsync()
    {
        Debug.Log("Entering MenuState");
        await EventBus.PublishAsync(new ScreenChangeEvent(ScreenType.Menu));
    }

    public async Task ExitAsync()
    {
        Debug.Log("Exiting MenuState");
        await Task.CompletedTask;
    }

}
public class GamePlayState : IGameState
{
    private readonly LevelConfig levelConfig;

    public GamePlayState(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;
    }

    public async Task EnterAsync()
    {
        Debug.Log("Entering GamePlayState");

        await EventBus.PublishAsync(new ScreenChangeEvent(ScreenType.Game));//panelmanager paneli değiştirir.
        await EventBus.PublishAsync(new GameLoadEvent(levelConfig));//tilelar sırayla oluşturulur

        GameTimer.Instance.StartTimer(PlayerPrefsService.TimerStart);
    }

    public async Task ExitAsync()
    {
        Debug.Log("Exiting GamePlayState");
        GameTimer.Instance.Pause();
        await Task.CompletedTask;
    }
}

public class GamePauseState : IGameState
{
    public async Task EnterAsync()
    {
        Debug.Log("Game paused");
        GameTimer.Instance.Pause();
        await EventBus.PublishAsync(new GamePauseEvent(GamePauseType.Fail_UserQuit));
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
        Debug.Log("Game won!");
        GameTimer.Instance.StopTimer();
        await EventBus.PublishAsync(new GameWinEvent());
        await EventBus.PublishAsync(new ScreenChangeEvent(ScreenType.Win));
    }

    public async Task ExitAsync()
    {
        await Task.CompletedTask;
    }
}
public class GameFailState : IGameState
{
    private readonly GamePauseType reason;

    public GameFailState(GamePauseType reason)
    {
        this.reason = reason;
    }

    public async Task EnterAsync()
    {
        Debug.Log($"Game failed: {reason}");
        await EventBus.PublishAsync(new GamePauseEvent(reason));

        ScreenType screen = reason switch
        {
            GamePauseType.Fail_TimeOver => ScreenType.Fail_TimeOver,
            GamePauseType.Fail_NoMoves => ScreenType.Fail_NoMoves,
            _ => ScreenType.Menu
        };

        await EventBus.PublishAsync(new ScreenChangeEvent(screen));
    }

    public async Task ExitAsync()
    {
        await Task.CompletedTask;
    }
}




public static class GameStateHelper
{
    public static async Task FailDueToTime()
    {
        await GameStateMachine.ChangeStateAsync(new GameFailState(GamePauseType.Fail_TimeOver));
    }

    public static async Task FailDueToMoves()
    {
        await GameStateMachine.ChangeStateAsync(new GameFailState(GamePauseType.Fail_NoMoves));
    }

    public static async Task Win()
    {
        await GameStateMachine.ChangeStateAsync(new GameWinState());
    }
}


