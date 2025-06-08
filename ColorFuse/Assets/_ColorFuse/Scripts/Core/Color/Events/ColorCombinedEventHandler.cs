using UnityEngine;

public class ColorCombineHandler : MonoBehaviour, IGameSystem
{
    public void Initialize()
    {
        Debug.Log("ColorCombineHandler: Initialized and subscribing to ColorCombinedEvent.");
        EventBus.Subscribe<ColorEvents.TryColorCombinedEvent>(OnColorCombined);
        EventBus.Subscribe<ColorEvents.MixColorFormedEvent>(OnMixColorCombine);
        EventBus.Subscribe<ColorEvents.WhiteColorFormedEvent>(OnWhiteCombine);
        EventBus.Subscribe<ColorEvents.InvalidCombinationEvent>(OnInvalidCombine);
    }

    public void Shutdown()
    {
        Debug.Log("ColorCombineHandler: Shutdown and unsubscribing from ColorCombinedEvent.");
        EventBus.Unsubscribe<ColorEvents.TryColorCombinedEvent>(OnColorCombined);
        EventBus.Unsubscribe<ColorEvents.MixColorFormedEvent>(OnMixColorCombine);
        EventBus.Unsubscribe<ColorEvents.WhiteColorFormedEvent>(OnWhiteCombine);
        
        EventBus.Subscribe<ColorEvents.InvalidCombinationEvent>(OnInvalidCombine);
    }

    void OnColorCombined(ColorEvents.TryColorCombinedEvent e)
    {

        if (e.SourceTile == null || e.TargetTile == null)
        {
            Debug.LogError("ColorCombinedEvent contains null tiles!");
            return;
        }
        Debug.Log($"ColorCombineHandler: Combining color at {e.SourceTile.name} → {e.TargetTile.name}, result: {e.Result}");
        if (!e.Result.IsValidColor)
        {
            Debug.Log("invalid combinedColor");
            return;
        }

        if (ColorVector.IsTheSame(e.SourceTile.PeekColor(), e.TargetTile.PeekColor()))
        {
            EventBus.Publish(new ColorEvents.InvalidCombinationEvent());
            return;
        }
        // Üst renkler her durumda çıkarılıyor
        e.SourceTile.PopTopColor();
        e.TargetTile.PopTopColor();


        if (e.Result.IsWhite)
        {
            Debug.Log("ColorCombineHandler: White color formed!");
            EventBus.Publish(new ColorEvents.WhiteColorFormedEvent(e.TargetTile));
            return;
        }
        if (e.Result.IsBaseColor || e.Result.IsIntermediateColor)
        {
            EventBus.Publish(new ColorEvents.MixColorFormedEvent(e.TargetTile, e.Result));
        }
    }


    void OnWhiteCombine(ColorEvents.WhiteColorFormedEvent e)
    {
        EventBus.Publish(new TileEvents.WhiteTileCollectedEvent());
        Debug.Log("white collected");

    }
    void OnMixColorCombine(ColorEvents.MixColorFormedEvent e)
    {
        EventBus.Publish(new TileEvents.TileCombinedEvent(e.MixColorTile, e.Result));
        e.MixColorTile.PushColor(e.Result);
    }
    void OnInvalidCombine(ColorEvents.InvalidCombinationEvent e)
    {
        EventBus.Publish(new TileEvents.TileRelasedEvent());
    }
}
