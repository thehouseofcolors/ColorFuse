using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using System;

public class LevelHUDController : MonoBehaviour, IGameSystem
{
    [SerializeField] TextMeshProUGUI moveText, timerText;
    private IDisposable _moveSub;
    private IDisposable _timerSub;

    public void Initialize()
    {
        _moveSub = EventBus.Subscribe<UpdateMoveCountUIEvent>(OnMoveUpdate);
        _timerSub = EventBus.Subscribe<UpdateTimerUIEvent>(OnTimerUpdate);
    }

    public void Shutdown()
    {
        _moveSub?.Dispose();
        _timerSub?.Dispose();
    }

    private async Task OnMoveUpdate(UpdateMoveCountUIEvent evt)
    {
        moveText.text =$"Moves: {evt.MoveCount}";
        await Task.CompletedTask;
    }

    private async Task OnTimerUpdate(UpdateTimerUIEvent evt)
    {
        timerText.text = $"Time: {Mathf.CeilToInt(evt.RemainingTime)}";
        await Task.CompletedTask;
    }


}


