

public class LevelCompletedEvent : IGameEvent
{
    public int LevelNumber;

    public int TimeRecord;
    public bool isSccessfull;


    public LevelCompletedEvent(int level, int time, bool result)
    {
        LevelNumber = level;
        TimeRecord = time;
        isSccessfull = result;
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

