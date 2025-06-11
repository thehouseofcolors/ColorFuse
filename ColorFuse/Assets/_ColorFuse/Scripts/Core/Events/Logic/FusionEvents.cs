
namespace FusionEvents
{
    public struct ColorMergeRequestedEvent : IGameEvent
    {
        public Tile SourceTile;
        public Tile TargetTile;

        public ColorMergeRequestedEvent(Tile source, Tile target)
        {
            SourceTile = source;
            TargetTile = target;
        }
    }
    public struct ColorMergeApprovedEvent : IGameEvent
    {
        public Tile Source => mergeEvent.SourceTile;
        public Tile Target => mergeEvent.TargetTile;

        public ColorMergeRequestedEvent mergeEvent;
        public ColorMergeApprovedEvent(ColorMergeRequestedEvent requestedEvent)
        {
            mergeEvent = requestedEvent;
        }
    }

    public struct InvalidCombinationEvent : IGameEvent
    {

    }
    public struct WhiteColorFormedEvent : IGameEvent
    {
        
    }

    




    }

namespace FusionEffectEvents
{
    public struct PlayMergeEffectEvent : IGameEvent
    {
        public Tile SourceTile;
        public Tile TargetTile;

        public PlayMergeEffectEvent(Tile source, Tile target)
        {
            SourceTile = source;
            TargetTile = target;
        }
    }

    public struct PlayInvalidCombinationEffectEvent : IGameEvent
    {
    }
}
