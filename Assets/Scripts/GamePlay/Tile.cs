
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using GameEvents;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    // Serialized Fields
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject transferEffectPrefab;
    [SerializeField] private float transferDuration = 0.35f;
    [SerializeField] private float maxTransferDistance = 2f;

    // Properties
    private Stack<ColorVector> colorStack = new Stack<ColorVector>();
    public bool IsEmpty => colorStack.Count == 0;
    public bool CanSelectable { get; set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    
    // State Management
    private enum SelectionState { None, Highlighted, Selected }
    private SelectionState selectionState = SelectionState.None;
    
    // Animation Parameters
    private const float HIGHLIGHT_SCALE = 1.2f;
    private const float SELECT_SCALE = 1.1f;
    private const float PUSH_SCALE = 0.8f;
    private const float POP_SCALE = 1.2f;
    private const float ANIMATION_DURATION = 0.15f;

    #region Unity Lifecycle
    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }
    #endregion

    #region Public Methods
    public void SetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public ColorVector PopTopColor()
    {
        if (IsEmpty) return ColorVector.Null;
        var color = colorStack.Pop();
        UpdateVisual();
        return color;
    }

    public void PushColor(ColorVector color)
    {
        colorStack.Push(color);
        UpdateVisual();
    }

    public ColorVector PeekColor()
    {
        if (IsEmpty)
        {
            Debug.LogWarning("Attempted to peek empty tile");
            return ColorVector.Null;
        }
        return colorStack.Peek();
    }
    public void UpdateVisual()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.color = IsEmpty ? Color.clear : PeekColor().ToUnityColor();
        SetSelectableStatus(!IsEmpty);
    }
    public void SetHighlight(bool on)
    {
        selectionState = on ? SelectionState.Highlighted : SelectionState.None;
        transform.DOScale(on ? HIGHLIGHT_SCALE : 1f, ANIMATION_DURATION)
                .SetEase(Ease.OutBack);
    }
    #endregion

    #region Selection Handling


    public async Task HandleSelection()
    {
        if (!CanSelectable) return;

        selectionState = SelectionState.Selected;
        
        var selectSequence = DOTween.Sequence()
            .Append(transform.DOScale(SELECT_SCALE, ANIMATION_DURATION/2))
            .Join(spriteRenderer.DOColor(
                Color.Lerp(spriteRenderer.color, Color.white, 0.3f), 
                ANIMATION_DURATION/2))
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);

        await selectSequence.AsyncWaitForCompletion();
        await EventBus.PublishAsync(new TileSelectionEvent(this));
        
        selectionState = SelectionState.Highlighted;
    }
    #endregion

    #region Color Transfer
    public async Task SafeTransferTo(Tile target)
    {
        try
        {
            if (!ValidateTransferTarget(target)) return;
            await TransferColorTo(target);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Transfer failed: {ex.Message}");
            // await EventBus.PublishAsync(new TileTransferFailedEvent(this, target)); böyle şeyimiz yok

        }
    }

    public async Task TransferColorTo(Tile targetTile)
    {
        if (IsEmpty || targetTile == null) return;

        var color = PeekColor();
        var effect = CreateTransferEffect(color);

        await AnimateColorTransfer(targetTile, effect);
        
        ProcessColorMerge(targetTile, color);
        ReleaseEffect(effect);
        await targetTile.PlayReceiveAnimation();
    }

    private bool ValidateTransferTarget(Tile target)
    {
        if (target == null || this == target) return false;
        if (Vector2.Distance(transform.position, target.transform.position) > maxTransferDistance)
        {
            Debug.LogWarning("Transfer distance too large");
            return false;
        }
        return true;
    }
    #endregion

    #region Animation Methods
    public Tween PlayPopAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(POP_SCALE, ANIMATION_DURATION).SetEase(Ease.OutBack));
        sequence.Append(transform.DOScale(1f, ANIMATION_DURATION).SetEase(Ease.InBack));
        return sequence;
    }

    public Tween PlayPushAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(PUSH_SCALE, ANIMATION_DURATION).SetEase(Ease.OutBack));
        sequence.Append(transform.DOScale(1f, ANIMATION_DURATION).SetEase(Ease.InBack));
        return sequence;
    }

    public async Task PlayReceiveAnimation()
    {
        await PlayPushAnimation().AsyncWaitForCompletion();
    }

    // public async Task PlayMergeEffect(ColorVector combinedColor)
    // {
    //     var particles = GetComponent<ParticleSystem>();
    //     if (particles != null)
    //     {
    //         var main = particles.main;
    //         main.startColor = combinedColor.ToUnityColor();
    //         particles.Play();
    //         await Task.Delay(Mathf.RoundToInt(main.duration * 1000));
    //     }
    // }
    #endregion

    #region Private Helpers
    

    private void SetSelectableStatus(bool selectable)
    {
        CanSelectable = selectable;
    }

    private GameObject CreateTransferEffect(ColorVector color)
    {
        var effect = GetEffectInstance();
        effect.transform.position = transform.position;
        var renderer = effect.GetComponent<SpriteRenderer>();
        renderer.color = color.ToUnityColor();
        renderer.sortingOrder = 10;
        return effect;
    }

    private async Task AnimateColorTransfer(Tile target, GameObject effect)
    {
        var transferSequence = DOTween.Sequence()
            .Append(effect.transform.DOMove(target.transform.position, transferDuration))
            .Join(effect.transform.DOScale(1.2f, transferDuration/2))
            .Append(effect.transform.DOScale(0f, transferDuration/2))
            .SetEase(Ease.InOutSine);

        PopTopColor();
        await transferSequence.AsyncWaitForCompletion();
    }

    private void ProcessColorMerge(Tile target, ColorVector color)
    {
        var targetColor = target.IsEmpty ? color : ColorFusion.Fuse(color, target.PeekColor());
        target.PushColor(targetColor);
    }

    private GameObject GetEffectInstance()
    {
        // Implementation with object pooling would go here
        return Instantiate(transferEffectPrefab, transform.position, Quaternion.identity);
    }

    private void ReleaseEffect(GameObject effect)
    {
        // Implementation with object pooling would go here
        Destroy(effect);
    }
    #endregion
}

