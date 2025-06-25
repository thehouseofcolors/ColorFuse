
using System;
using System.Threading.Tasks;
using UnityEngine;

public static class GameStateMachine
{
    private static IGameState _currentState;
    private static IGameState _previousState;
    private static bool _isTransitioning;
    private static int _transitionCount;

    private static bool _logTransitions = true;
    private static bool _logTransitionTimes = false;
    private static float _minWarningTransitionTime = 0.1f;

    public static IGameState CurrentState => _currentState;
    public static IGameState PreviousState => _previousState;
    public static bool IsTransitioning => _isTransitioning;
    public static int TransitionCount => _transitionCount;

    public static async Task ChangeStateAsync(IGameState newState, StateTransitionOptions options = null)
    {

        Debug.Log($"From {_currentState?.GetType().Name} to {newState?.GetType().Name}");

        options ??= StateTransitionOptions.Default;

        if (_isTransitioning && !options.AllowNestedTransitions)
        {
            Debug.LogWarning($"State transition blocked - already transitioning. Current: {_currentState?.GetType().Name}, New: {newState?.GetType().Name}");
            return;
        }

        if (_currentState == newState && !options.AllowReentry)
        {
            if (_logTransitions)
                Debug.Log($"State re-entry blocked for {newState?.GetType().Name}");
            return;
        }

        _isTransitioning = true;
        _transitionCount++;

        System.Diagnostics.Stopwatch sw = null;
        if (_logTransitionTimes)
        {
            sw = System.Diagnostics.Stopwatch.StartNew();
        }

        try
        {
            if (_currentState != null && _logTransitions)
                Debug.Log($"Exiting state: {_currentState.GetType().Name}");

            await _currentState?.ExitAsync();

            _previousState = _currentState;
            _currentState = newState;

            if (_currentState != null && _logTransitions)
                Debug.Log($"Entering state: {_currentState.GetType().Name}");

            await _currentState?.EnterAsync();

            if (sw != null)
            {
                sw.Stop();
                var elapsed = sw.Elapsed.TotalSeconds;
                if (elapsed > _minWarningTransitionTime)
                {
                    Debug.LogWarning($"State transition took {elapsed:0.000}s - {_previousState?.GetType().Name} → {_currentState?.GetType().Name}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"State transition failed: {ex}");
            if (options.RevertOnFailure)
            {
                Debug.LogWarning($"Reverting to previous state: {_previousState?.GetType().Name}");
                await ChangeStateAsync(_previousState, new StateTransitionOptions { AllowNestedTransitions = true });
            }
        }
        finally
        {
            _isTransitioning = false;
        }
    }

    public static async Task SetInitialStateAsync(IGameState initialState)
    {
        if (_currentState != null)
        {
            Debug.LogError("Initial state already set.");
            return;
        }

        _currentState = initialState;
        _previousState = null;

        if (_logTransitions)
            Debug.Log($"[GameStateMachine] Setting initial state: {_currentState.GetType().Name}");

        await _currentState.EnterAsync();
    }

    public static bool IsInState<T>() where T : IGameState => _currentState is T;

    public static bool TryGetCurrentState<T>(out T state) where T : class, IGameState
    {
        state = _currentState as T;
        return state != null;
    }

    public static string CurrentStateName => _currentState?.GetType().Name ?? "None";
    public static string PreviousStateName => _previousState?.GetType().Name ?? "None";

    public class StateTransitionOptions
    {
        public static StateTransitionOptions Default => new StateTransitionOptions();

        public bool AllowNestedTransitions = false;
        public bool AllowReentry = false;
        public bool RevertOnFailure = true;
    }
}
