
using NUnit.Framework;

public static class LevelEvents
{
    
    public struct LevelStartedEvent : IGameEvent
    {
        public LevelConfig LevelConfig;

        public LevelStartedEvent(LevelConfig levelConfig)
        {
            LevelConfig = levelConfig;
        }
    }

    public struct LevelCompletedEvent : IGameEvent
    {
        public int LevelNumber;
        public float TimeRecord;
        public bool IsSuccess;
        public LevelCompletedEvent(int level, float time, bool isSuccess)
        {
            LevelNumber = level;
            TimeRecord = time;
            IsSuccess = isSuccess;
        }
    }


    public struct LevelUnlockedEvent : IGameEvent
    {
        public int LevelNumber;
        public LevelUnlockedEvent(int level)
        {
            LevelNumber = level;
        }
    }




}

