using System.Threading.Tasks;
using UnityEngine;
using FusionEvents;

public class FusionProcessor : MonoBehaviour, IGameSystem
{
    public void Initialize()
    {
        EventBus.Subscribe<ColorMergeRequestedEvent>(OnMergeRequested);
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<ColorMergeRequestedEvent>(OnMergeRequested);
    }

    private async Task OnMergeRequested(ColorMergeRequestedEvent e)
    {
        Debug.Log($"Processing merge request: {e.SourceTile} + {e.TargetTile}");
        await Task.Delay(50); // Gerekirse kısa bekleme (oyun hızına göre ayarla)

        var colorA = e.SourceTile.PeekColor();
        var colorB = e.TargetTile.PeekColor();
        var result = ColorFusion.Merge(colorA, colorB);

        if (!result.IsValidColor)
        {
            await EventBus.PublishAsync(new InvalidCombinationEvent());
            await EventBus.PublishAsync(new FusionEffectEvents.PlayInvalidCombinationEffectEvent());
            return;
        }

        await EventBus.PublishAsync(new ColorMergeApprovedEvent(e));
        await EventBus.PublishAsync(new FusionEffectEvents.PlayMergeEffectEvent(e.SourceTile, e.TargetTile));
    }
}
