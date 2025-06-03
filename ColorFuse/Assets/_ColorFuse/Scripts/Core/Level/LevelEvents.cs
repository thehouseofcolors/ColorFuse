
using NUnit.Framework;

public static class LevelEvents
{
    public struct StartButtonPressed : IGameEvent {}
    public struct RestartButtonPressed : IGameEvent {}
    
    public struct LevelStartedEvent : IGameEvent
    {
        public int Level;
        public bool IsRestart; // yeni özellik: restart mı yoksa ilk başlatma mı?

        public LevelStartedEvent(int level, bool isRestart = false)
        {
            Level = level;
            IsRestart = isRestart;
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



    public struct LevelFailedEvent : IGameEvent
    {

    }


    public struct LevelSuccessEvent : IGameEvent
    {

    }


}

