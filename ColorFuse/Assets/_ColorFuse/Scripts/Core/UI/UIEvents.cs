

public static class UIEvents
{

    public struct LevelButtonPressed : IGameEvent
    {
        public LevelConfig LevelConfig;

        public LevelButtonPressed(LevelConfig levelConfig)
        {
            LevelConfig = levelConfig;
        }
    }


    public struct NextButtonPressed : IGameEvent
    { 
        public int NextLevel;
        public NextButtonPressed(int currentLevel)
        {
            NextLevel = currentLevel + 1;
        }
    } 

    public struct RestartButtonPressed : IGameEvent
    {
        public int CurrentLevel;
        public RestartButtonPressed(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
    }

    public struct ShowMainMenuEvent : IGameEvent { }
    public struct ShowLevelHUDPanelEvent : IGameEvent { }
    public struct ShowSuccessPanelEvent : IGameEvent { }
    public struct ShowFailPanelEvent : IGameEvent { }
    

}
