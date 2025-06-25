using System;
using System.Threading;
using System.Threading.Tasks;
using GameEvents;
using UnityEngine;

public class GameTimer : Singleton<GameTimer>
{
    private float _remainingTime;
    private bool _isRunning;
    private bool _isPaused;
    private CancellationTokenSource _cts;

    public void Disable()
    {
        StopTimer();
    }

    public void StartTimer(float duration)
    {
        if (duration <= 0)
        {
            Debug.LogWarning("Timer duration must be positive");
            return;
        }

        StopTimer();
        _remainingTime = duration;
        _isRunning = true;
        _isPaused = false;
        _cts = new CancellationTokenSource();

        _ = RunTimerAsync(_cts.Token);
    }

    public void StopTimer()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        _isRunning = false;
        _isPaused = false;
    }

    public void Pause()
    {
        if (_isRunning && !_isPaused)
            _isPaused = true;
    }

    public void Resume()
    {
        if (_isRunning && _isPaused)
            _isPaused = false;
    }

    public async Task AddTime(float seconds)
    {
        if (_isRunning)
        {
            _remainingTime += seconds;
            await Task.CompletedTask;
        }
    }

    private async Task RunTimerAsync(CancellationToken token)
    {
        try
        {
            while (_remainingTime > 0 && !token.IsCancellationRequested)
            {
                if (_isPaused)
                {
                    EventBus.PublishSync(new UpdateTimerUIEvent(_remainingTime));
                    await Task.Delay(100, token);
                    continue;
                }

                await Task.Delay(1000, token);
                if (token.IsCancellationRequested) break;

                _remainingTime -= 1f;
                EventBus.PublishSync(new UpdateTimerUIEvent(_remainingTime));
            }

            if (_remainingTime <= 0 && !token.IsCancellationRequested)
            {
                // await GameStateHelper.FailDueToTime();
            }
        }
        catch (OperationCanceledException)
        {
            // Timer durduruldu
        }
        finally
        {
            _isRunning = false;
        }
    }

    public float RemainingTime => _remainingTime;
    public bool IsRunning => _isRunning;
    public bool IsPaused => _isPaused;
}
