
public static class ColorEvents
{
    public struct TryColorCombinedEvent : IGameEvent
    {
        public Tile SourceTile;
        public Tile TargetTile;
        public ColorVector Result;

        public TryColorCombinedEvent(Tile source, Tile target, ColorVector result)
        {
            SourceTile = source;
            TargetTile = target;
            Result = result;
        }
    }
    public struct MixColorFormedEvent : IGameEvent
    {
        public Tile MixColorTile;
        public ColorVector Result;
        public MixColorFormedEvent(Tile resulttile, ColorVector result)
        {
            MixColorTile = resulttile;
            Result = result;

        }
    }
    public struct WhiteColorFormedEvent : IGameEvent
    {
        public Tile WhiteTile;
        public WhiteColorFormedEvent(Tile tile) => WhiteTile = tile;
    }

    public struct InvalidCombinationEvent : IGameEvent {}




}
