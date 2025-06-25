using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using DG.Tweening;
using TMPro;
using System;

public class StartPanelController : MonoBehaviour, IPanel
{
    [Header("References")]
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panelContent;
    [SerializeField] private TextMeshProUGUI levelTitleText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float scaleDuration = 0.7f;
    [SerializeField] private float buttonPulseAmount = 0.1f;
    [SerializeField] private float buttonPulseDuration = 1f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [SerializeField] private float titleBounceDelay = 0.3f;

    private Vector3 _originalScale;
    private Sequence _pulseSequence;
    private IDisposable _levelUpdateSub;

    private void Awake()
    {
        // Initialize components
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        _originalScale = panelContent.localScale;

        // Set initial state
        canvasGroup.alpha = 0;
        panelContent.localScale = Vector3.zero;
        gameObject.SetActive(false);

        // Setup button listener
        startButton.onClick.AddListener(OnStartClicked);

        // Subscribe to level updates
        _levelUpdateSub = EventBus.Subscribe<LevelInfoUpdateEvent>(OnLevelInfoUpdated);

    }

    public async Task ShowAsync(object transitionData)
    {
        gameObject.SetActive(true);

        // Reset animation if already playing
        StopAllAnimations();

        // Create show animation sequence
        var sequence = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1, fadeDuration))
            .Join(panelContent.DOScale(_originalScale, scaleDuration).SetEase(scaleEase));

        // Animate level title with delay
        if (levelTitleText != null)
        {
            levelTitleText.transform.localScale = Vector3.zero;
            sequence.Insert(titleBounceDelay,
                levelTitleText.transform.DOScale(Vector3.one, scaleDuration * 0.7f)
                    .SetEase(Ease.OutBack));
        }

        await sequence.AsyncWaitForCompletion();

        // Start button pulse animation
        StartButtonPulseAnimation();
    }

    public async Task HideAsync()
    {
        StopAllAnimations();

        // Create hide animation
        var sequence = DOTween.Sequence()
            .Append(canvasGroup.DOFade(0, fadeDuration * 0.7f))
            .Join(panelContent.DOScale(Vector3.zero, scaleDuration * 0.5f).SetEase(Ease.InBack))
            .OnComplete(() => gameObject.SetActive(false));

        await sequence.AsyncWaitForCompletion();
    }

    private void OnStartClicked()
    {
        Debug.Log("start clicked");
        EventBus.PublishSync(new GameStartRequestedEvent());
        PlayButtonClickFeedback(startButton.transform);
    }

    private void OnLevelInfoUpdated(LevelInfoUpdateEvent evt)
    {
        levelText.text = $"Level {evt.LevelNumber}";
    }

    private void StartButtonPulseAnimation()
    {
        _pulseSequence = DOTween.Sequence()
            .Append(startButton.transform.DOScale(Vector3.one * (1 + buttonPulseAmount), buttonPulseDuration / 2).SetEase(Ease.InOutSine))
            .Append(startButton.transform.DOScale(Vector3.one, buttonPulseDuration / 2).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Restart);
    }

    private void PlayButtonClickFeedback(Transform buttonTransform)
    {
        buttonTransform.DOKill();
        buttonTransform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 2, 0.5f)
            .OnComplete(() => buttonTransform.localScale = Vector3.one);
    }

    private void StopAllAnimations()
    {
        _pulseSequence?.Kill();
        canvasGroup.DOKill();
        panelContent.DOKill();
        if (levelTitleText != null) levelTitleText.transform.DOKill();
        startButton.transform.DOKill();
    }

    private void OnDestroy()
    {
        // Clean up
        startButton.onClick.RemoveAllListeners();
        _levelUpdateSub?.Dispose();
        StopAllAnimations();
    }
    

}


