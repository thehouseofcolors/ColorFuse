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
        public Tile SelectedTile;

        public TileEmptiedEvent(Tile tile)
        {
            SelectedTile = tile;
        }
    }

    public struct TileRelasedEvent : IGameEvent
    {
        public Tile RelasedTile;

        public TileRelasedEvent(Tile tile)
        {
            RelasedTile = tile;
        }

    }



    
}
