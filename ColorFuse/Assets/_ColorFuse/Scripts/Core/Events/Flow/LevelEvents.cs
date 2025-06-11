
namespace LevelEvents
{
    public struct LevelLoadEvent : IGameEvent
    {
        public LevelMetadata levelMetadata;
        public LevelLoadEvent(LevelMetadata levelMetadata)
        {
            this.levelMetadata = levelMetadata;
        }
    }
    
    // public struct LevelStartedEvent : IGameEvent
    // {
    //     public LevelMetadata levelMetadata;
    //     public LevelStartedEvent(LevelMetadata levelMetadata)
    //     {
    //         this.levelMetadata = levelMetadata;
    //     }
    // }


    // public struct LevelCompletedEvent : IGameEvent
    // {
    //     public int LevelNumber;
    //     public bool IsSuccess;
    //     public LevelCompletedEvent(int level, bool isSuccess)
    //     {
    //         LevelNumber = level;
    //         IsSuccess = isSuccess;
    //     }
    // }


    public struct LevelUnlockedEvent : IGameEvent
    {
        public int LevelNumber;
        public LevelUnlockedEvent(int level)
        {
            LevelNumber = level;
        }
    }




}

