using UnityEngine;

public class ColorCombineHandler : MonoBehaviour, IGameSystem
{
    public void Initialize()
    {
        Debug.Log("ColorCombineHandler: Initialized and subscribing to ColorCombinedEvent.");
        EventBus.Subscribe<ColorEvents.TryColorCombinedEvent>(OnColorCombined);
        EventBus.Subscribe<ColorEvents.MixColorFormedEvent>(OnMixColorCombine);
        EventBus.Subscribe<ColorEvents.WhiteColorFormedEvent>(OnWhiteCombine);
    }

    public void Shutdown()
    {
        Debug.Log("ColorCombineHandler: Shutdown and unsubscribing from ColorCombinedEvent.");
        EventBus.Unsubscribe<ColorEvents.TryColorCombinedEvent>(OnColorCombined);
        EventBus.Unsubscribe<ColorEvents.MixColorFormedEvent>(OnMixColorCombine);
        EventBus.Unsubscribe<ColorEvents.WhiteColorFormedEvent>(OnWhiteCombine);
    }

    void OnColorCombined(ColorEvents.TryColorCombinedEvent e)
    {

        if (e.SourceTile == null || e.TargetTile == null)
        {
            Debug.LogError("ColorCombinedEvent contains null tiles!");
            return;
        }

        Debug.Log($"ColorCombineHandler: Combining color at {e.SourceTile.name} → {e.TargetTile.name}, result: {e.Result}");
        if (!e.Result.IsValidColor) return;
        
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
        
        Debug.Log("white collected");

    }
    void OnMixColorCombine(ColorEvents.MixColorFormedEvent e)
    {
        e.MixColorTile.PushColor(e.Result);
    }
    
}
