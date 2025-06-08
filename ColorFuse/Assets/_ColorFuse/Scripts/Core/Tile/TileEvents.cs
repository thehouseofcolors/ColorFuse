// Event tanımları: TileEvents.cs
public static class TileEvents
{
    public struct TileSelectedEvent : IGameEvent
    {
        public Tile SelectedTile;

        public TileSelectedEvent(Tile tile)
        {
            SelectedTile = tile;
        }
    }

    public struct TileEmptiedEvent : IGameEvent
    {
        public Tile EmptiedTile;

        public TileEmptiedEvent(Tile tile)
        {
            EmptiedTile = tile;
        }
    }

    public struct TileRelasedEvent : IGameEvent
    {
        // public Tile RelasedTile;

        // public TileRelasedEvent(Tile tile)
        // {
        //     RelasedTile = tile;
        // }

    }

    public struct TileCombinedEvent : IGameEvent
    {
        public Tile CombinedTile;
        public ColorVector CombinedColor;
        public TileCombinedEvent(Tile tile, ColorVector colorVector)
        {
            CombinedTile = tile;
            CombinedColor = colorVector;
        }

    }

    public struct WhiteTileCollectedEvent : IGameEvent {}
}
