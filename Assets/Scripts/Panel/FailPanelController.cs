using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using DG.Tweening;
using TMPro;

public class FailPanelController : MonoBehaviour, IPanel
{
    [Header("References")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panelContent;
    [SerializeField] private TextMeshProUGUI failReasonText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float scaleInDuration = 0.7f;
    [SerializeField] private float bounceIntensity = 0.2f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [SerializeField] private float buttonDelay = 0.3f;

    private Vector3 _originalScale;
    private Sequence _currentAnimation;

    private void Awake()
    {
        // Initialize components
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        _originalScale = panelContent.localScale;
        
        // Set initial state
        canvasGroup.alpha = 0;
        panelContent.localScale = Vector3.zero;
        gameObject.SetActive(false);
        
        // Setup button listeners
        restartButton.onClick.AddListener(OnRestartClicked);
        menuButton.onClick.AddListener(OnMenuClicked);
    }

    public async Task ShowAsync(object transitionData)
    {
        gameObject.SetActive(true);
        
        // Set fail reason text if provided
        if (transitionData is string reason && !string.IsNullOrEmpty(reason))
        {
            failReasonText.text = reason;
        }
        
        // Reset animation if already playing
        _currentAnimation?.Kill();
        
        // Create show animation sequence
        _currentAnimation = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1, fadeInDuration))
            .Join(panelContent.DOScale(_originalScale * 1.1f, scaleInDuration * 0.7f).SetEase(Ease.OutQuad))
            .Append(panelContent.DOScale(_originalScale, scaleInDuration * 0.3f).SetEase(Ease.InOutBounce))
            .OnComplete(() => _currentAnimation = null);

        // Animate buttons with slight delay
        await Task.Delay((int)(buttonDelay * 1000));
        PlayButtonAppearAnimation(restartButton.transform);
        await Task.Delay(100);
        PlayButtonAppearAnimation(menuButton.transform);
        
        await _currentAnimation.AsyncWaitForCompletion();
    }

    public async Task HideAsync()
    {
        // Reset animation if already playing
        _currentAnimation?.Kill();
        
        // Create hide animation
        _currentAnimation = DOTween.Sequence()
            .Append(canvasGroup.DOFade(0, fadeInDuration * 0.7f))
            .Join(panelContent.DOScale(Vector3.zero, scaleInDuration * 0.5f).SetEase(Ease.InBack))
            .OnComplete(() => {
                gameObject.SetActive(false);
                _currentAnimation = null;
            });
        
        await _currentAnimation.AsyncWaitForCompletion();
    }

    private void OnRestartClicked()
    {
        EventBus.PublishSync(new LevelRestartRequestedEvent());
        PlayButtonClickFeedback(restartButton.transform);
    }

    private void OnMenuClicked()
    {
        EventBus.PublishSync(new MenuRequestedEvent());
        PlayButtonClickFeedback(menuButton.transform);
    }

    private void PlayButtonAppearAnimation(Transform buttonTransform)
    {
        buttonTransform.localScale = Vector3.zero;
        buttonTransform.DOScale(Vector3.one, 0.4f)
            .SetEase(Ease.OutBack)
            .SetDelay(Random.Range(0.1f, 0.3f));
    }

    private void PlayButtonClickFeedback(Transform buttonTransform)
    {
        buttonTransform.DOKill();
        buttonTransform.DOPunchScale(Vector3.one * bounceIntensity, 0.3f, 2, 0.5f);
    }

    private void OnDestroy()
    {
        // Clean up
        restartButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        _currentAnimation?.Kill();
    }
}

