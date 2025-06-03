using UnityEngine;

public class ColorCombineHandler : MonoBehaviour, IGameSystem
{
    public void Initialize()
    {
        Debug.Log("ColorCombineHandler: Initialized and subscribing to ColorCombinedEvent.");
        EventBus.Subscribe<TileEvents.ColorCombinedEvent>(OnColorCombined);
        EventBus.Subscribe<TileEvents.WhiteColorFormedEvent>(OnWhiteCombine);
    }

    public void Shutdown()
    {
        Debug.Log("ColorCombineHandler: Shutdown and unsubscribing from ColorCombinedEvent.");
        EventBus.Unsubscribe<TileEvents.ColorCombinedEvent>(OnColorCombined);
        EventBus.Unsubscribe<TileEvents.WhiteColorFormedEvent>(OnWhiteCombine);
    }

    private void OnColorCombined(TileEvents.ColorCombinedEvent e)
    {

        if (e.SourceTile == null || e.TargetTile == null)
        {
            Debug.LogError("ColorCombinedEvent contains null tiles!");
            return;
        }

        Debug.Log($"ColorCombineHandler: Combining color at {e.SourceTile.name} → {e.TargetTile.name}, result: {e.Result}");

        // Üst renkler her durumda çıkarılıyor
        e.SourceTile.PopTopColor();
        e.TargetTile.PopTopColor();
        e.TargetTile.PushColor(e.Result);

        EventBus.Publish(new TileEvents.TileStateChangedEvent());

        if (e.Result.IsWhite)
        {
            Debug.Log("ColorCombineHandler: White color formed!");
            EventBus.Publish(new TileEvents.WhiteColorFormedEvent(e.TargetTile));
            return;
        }
    }
    public void OnWhiteCombine(TileEvents.WhiteColorFormedEvent e)
    {
        e.TargetTile.PopTopColor();
        Debug.Log("white collected");
    }
}
