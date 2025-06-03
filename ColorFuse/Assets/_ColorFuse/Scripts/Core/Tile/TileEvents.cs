// Event tanımları: TileEvents.cs
public static class TileEvents
{
    public struct ColorCombinedEvent : IGameEvent
    {
        public Tile SourceTile;
        public Tile TargetTile;
        public ColorVector Result;

        public ColorCombinedEvent(Tile source, Tile target, ColorVector result)
        {
            SourceTile = source;
            TargetTile = target;
            Result = result;
        }
    }

    public struct WhiteColorFormedEvent : IGameEvent
    {
        public Tile TargetTile;
        public WhiteColorFormedEvent(Tile tile) => TargetTile = tile;
    }
    public struct TileSelectedEvent : IGameEvent
    {
        public Tile SelectedTile;

        public TileSelectedEvent(Tile tile)
        {
            SelectedTile = tile;
        }
    }


    public struct TileStateChangedEvent : IGameEvent { }



    
}
