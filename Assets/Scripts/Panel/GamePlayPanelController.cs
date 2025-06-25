using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using GameEvents;
using System;
using DG.Tweening;

// Handles panel show/hide transitions
public class GamePlayPanelController : MonoBehaviour, IPanel
{
    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float scaleDuration = 0.2f;
    
    private CanvasGroup _canvasGroup;
    private Vector3 _originalScale;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
        _originalScale = transform.localScale;
        InitializePanelState();
    }

    private void InitializePanelState()
    {
        _canvasGroup.alpha = 0;
        transform.localScale = _originalScale * 0.9f;
        gameObject.SetActive(false);
    }

    public async Task ShowAsync(object transitionData)
    {
        gameObject.SetActive(true);
        
        var sequence = DOTween.Sequence()
            .Append(_canvasGroup.DOFade(1, fadeDuration))
            .Join(transform.DOScale(_originalScale, scaleDuration).SetEase(Ease.OutBack));
        
        await sequence.AsyncWaitForCompletion();
    }

    public async Task HideAsync()
    {
        var sequence = DOTween.Sequence()
            .Append(_canvasGroup.DOFade(0, fadeDuration))
            .Join(transform.DOScale(_originalScale * 0.9f, scaleDuration).SetEase(Ease.InBack))
            .OnComplete(() => gameObject.SetActive(false));
            
        await sequence.AsyncWaitForCompletion();
    }
}



