
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using GameEvents;
using System;
using DG.Tweening;

// Handles game HUD logic and updates
public class GamePlayHUD : MonoBehaviour, IGameSystem
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moveText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Animation Settings")]
    [SerializeField] private float textUpdateScale = 1.1f;
    [SerializeField] private float textUpdateDuration = 0.2f;

    private IDisposable _moveSub;
    private IDisposable _timerSub;
    private Vector3 _originalMoveTextScale;
    private Vector3 _originalTimerTextScale;

    public void Initialize()
    {
        CacheOriginalScales();
        ResetUI();
        SubscribeToEvents();
    }

    private void CacheOriginalScales()
    {
        _originalMoveTextScale = moveText.transform.localScale;
        _originalTimerTextScale = timerText.transform.localScale;
    }

    private void ResetUI()
    {
        moveText.text = "Moves: 0";
        timerText.text = "Time: 0";
    }

    private void SubscribeToEvents()
    {
        _moveSub = EventBus.Subscribe<UpdateMoveCountUIEvent>(OnMoveUpdate);
        _timerSub = EventBus.Subscribe<UpdateTimerUIEvent>(OnTimerUpdate);
    }

    public void Shutdown()
    {
        _moveSub?.Dispose();
        _timerSub?.Dispose();
        _moveSub = null;
        _timerSub = null;
    }

    private async Task OnMoveUpdate(UpdateMoveCountUIEvent evt)
    {
        moveText.text = $"Moves: {evt.MoveCount}";
        await AnimateText(moveText.transform, _originalMoveTextScale);
    }

    private async Task OnTimerUpdate(UpdateTimerUIEvent evt)
    {
        timerText.text = $"Time: {Mathf.CeilToInt(evt.RemainingTime)}";

        // Add urgency effect when time is low
        if (evt.RemainingTime < 10f)
        {
            timerText.color = Color.red;
            await AnimateText(timerText.transform, _originalTimerTextScale, 1.3f);
        }
        else
        {
            timerText.color = Color.white;
            await AnimateText(timerText.transform, _originalTimerTextScale);
        }
    }

    private async Task AnimateText(Transform textTransform, Vector3 originalScale, float customScale = 0f)
    {
        float scaleAmount = customScale > 0 ? customScale : textUpdateScale;

        await textTransform.DOScale(originalScale * scaleAmount, textUpdateDuration / 2)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();

        await textTransform.DOScale(originalScale, textUpdateDuration / 2)
            .SetEase(Ease.InQuad)
            .AsyncWaitForCompletion();
    }

    private void OnDestroy()
    {
        Shutdown();
    }
}
