

public class LevelCompletedEvent : IGameEvent
{
    public int LevelNumber;

    public float TimeRecord;


    public LevelCompletedEvent(int level, float time)
    {
        LevelNumber = level;
        TimeRecord = time;
    }
    
}
public class LevelStartedEvent : IGameEvent
{
    public int LevelNumber;
    public LevelStartedEvent(int level)
    {
        LevelNumber = level;
        
    }
}

public class ColorCombinedEvent : IGameEvent
{
    public Tile SourceTile;
    public Tile TargetTile;
    public ColorVector ResultColor;

    public ColorCombinedEvent(Tile source, Tile target, ColorVector result)
    {
        SourceTile = source;
        TargetTile = target;
        ResultColor = result;
    }
}

public class WhiteColorFormedEvent : IGameEvent
{
    public Tile TargetTile;

    public WhiteColorFormedEvent(Tile tile)
    {
        TargetTile = tile;
    }
}

public class InvalidCombineEvent : IGameEvent
{
    public Tile TileA;
    public Tile TileB;

    public InvalidCombineEvent(Tile a, Tile b)
    {
        TileA = a;
        TileB = b;
    }
}

// public class GridGeneratedEvent : IGameEvent { } // payload içermeyen basit bir event

public class TileStateChangedEvent : IGameEvent { } // Gerekirse payload da eklenebilir

public struct ColorsAssignedEvent : IGameEvent { }
public struct GridGeneratedEvent : IGameEvent { }
public struct GameResetEvent  : IGameEvent { }

