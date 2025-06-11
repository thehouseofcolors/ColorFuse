
namespace GamePlayEvents
{
    public struct GameStartedEvent : IGameEvent
    {
        public LevelMetadata levelMetadata;
        public GameStartedEvent(LevelMetadata levelMetadata)
        {
            this.levelMetadata = levelMetadata;
        }
        
    }
    public struct GamePausedEvent : IGameEvent
    {

    }
    public struct GameResumedEvent : IGameEvent
    {

    }

}