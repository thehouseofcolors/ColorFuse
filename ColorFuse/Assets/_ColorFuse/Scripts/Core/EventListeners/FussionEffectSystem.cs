using System.Collections;
using UnityEngine;
using FusionEffectEvents;
using System.Threading.Tasks;

public class FusionEffectSystem : MonoBehaviour, IGameSystem
{
    public void Initialize()
    {
        EventBus.Subscribe<PlayMergeEffectEvent>(OnPlayMergeEffect);
        EventBus.Subscribe<PlayInvalidCombinationEffectEvent>(OnPlayInvalidCombinationEffect);
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<PlayMergeEffectEvent>(OnPlayMergeEffect);
        EventBus.Unsubscribe<PlayInvalidCombinationEffectEvent>(OnPlayInvalidCombinationEffect);
    }

    private Task OnPlayMergeEffect(PlayMergeEffectEvent e)
    {
        if (e.SourceTile != null)
            StartCoroutine(PlayPopAnimation(e.SourceTile));

        if (e.TargetTile != null)
            StartCoroutine(PlayPushAnimation(e.TargetTile));

        return Task.CompletedTask;
    }

    private Task OnPlayInvalidCombinationEffect(PlayInvalidCombinationEffectEvent e)
    {
        CameraShaker.Instance?.ShakeCamera();
        // Buraya başka animasyonlar da eklenebilir
        return Task.CompletedTask;
    }

    private IEnumerator PlayPopAnimation(Tile tile)
    {
        yield return tile.PlayPopAnimation(0.3f);
    }

    private IEnumerator PlayPushAnimation(Tile tile)
    {
        yield return tile.PlayPushAnimation(0.3f);
    }
}
