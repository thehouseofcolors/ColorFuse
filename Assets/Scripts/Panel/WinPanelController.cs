using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using DG.Tweening;

public class WinPanelController : MonoBehaviour, IPanel
{
    [Header("References")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform contentTransform;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float appearDelay = 0.2f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    private Vector3 _originalScale;
    private Sequence _currentAnimation;

    private void Awake()
    {
        // Initialize components
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        _originalScale = contentTransform.localScale;
        
        // Set initial state
        canvasGroup.alpha = 0;
        contentTransform.localScale = Vector3.zero;
        gameObject.SetActive(false);
        
        // Setup button listeners
        nextButton.onClick.AddListener(OnNextButtonClicked);
        menuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    public async Task ShowAsync(object transitionData)
    {
        gameObject.SetActive(true);
        
        // Reset animation if already playing
        _currentAnimation?.Kill();
        
        // Create show animation
        _currentAnimation = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1, fadeDuration))
            .Join(contentTransform.DOScale(_originalScale, scaleDuration).SetEase(scaleEase).SetDelay(appearDelay))
            .OnComplete(() => _currentAnimation = null);
        
        await _currentAnimation.AsyncWaitForCompletion();
    }

    public async Task HideAsync()
    {
        // Reset animation if already playing
        _currentAnimation?.Kill();
        
        // Create hide animation
        _currentAnimation = DOTween.Sequence()
            .Append(canvasGroup.DOFade(0, fadeDuration))
            .Join(contentTransform.DOScale(Vector3.zero, scaleDuration * 0.7f).SetEase(Ease.InBack))
            .OnComplete(() => {
                gameObject.SetActive(false);
                _currentAnimation = null;
            });
        
        await _currentAnimation.AsyncWaitForCompletion();
    }

    private void OnNextButtonClicked()
    {
        EventBus.PublishSync(new NextLevelRequestedEvent());
        PlayButtonClickFeedback(nextButton.transform);
    }

    private void OnMenuButtonClicked()
    {
        EventBus.PublishSync(new MenuRequestedEvent());
        PlayButtonClickFeedback(menuButton.transform);
    }

    private void PlayButtonClickFeedback(Transform buttonTransform)
    {
        // Play a quick scale punch animation when button is clicked
        buttonTransform.DOKill();
        buttonTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1, 0.5f);
    }

    private void OnDestroy()
    {
        // Clean up
        nextButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        _currentAnimation?.Kill();
    }
}

