namespace SelectionEvents
{
    public struct TileSelectedEvent : IGameEvent
    {
        public Tile SelectedTile;

        public TileSelectedEvent(Tile tile)
        {
            SelectedTile = tile;
        }
    }
    public struct ClearTileSelectionEvent : IGameEvent
    {

    }

    public struct PopTopColorEvent : IGameEvent
    {
        public Tile tile;
        public PopTopColorEvent(Tile tile)
        {
            this.tile = tile;
        }
    }
    public struct PushColorEvent : IGameEvent
    {
        public Tile tile;
        public PushColorEvent(Tile tile)
        {
            this.tile = tile;
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


}