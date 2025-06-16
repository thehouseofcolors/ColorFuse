

using System.Threading.Tasks;
using UnityEngine;
using GameEvents;
using NUnit.Framework.Interfaces;


public class FusionProcessor : MonoBehaviour, IGameSystem
{

    public void Initialize()
    {
        SubscribeEvents();
    }

    public void Shutdown()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventBus.Subscribe<TileFuseEvent>(OnTileFuse);
    }

    private void UnsubscribeEvents()
    {
        
    }
    async Task OnTileFuse(TileFuseEvent e)
    {
        var result = ColorFusion.Fuse(e.Source.PeekColor(), e.Target.PeekColor());
        if (!result.IsValidColor)
        {
            CameraShaker.Instance.ShakeCamera();
            return;
        }

        await ApproveMerge(e);
        if (result.IsWhite)
        {
            e.Target.PopTopColor();
        }

    }


    private ColorVector ProcessColorMerge(Tile source, Tile target)
    {
        return ColorFusion.Fuse(source.PeekColor(), target.PeekColor());
    }


    private async Task ApproveMerge(TileFuseEvent e)
    {
        PlayerPrefsService.RemainingMoves -= 1;
        await e.Source.TransferColorTo(e.Target);
        await EventBus.PublishAsync(new UpdateMoveCountUIEvent(PlayerPrefsService.RemainingMoves));
        await Task.CompletedTask;
    }

  
   
}

