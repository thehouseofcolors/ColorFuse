
namespace UIEvents
{
    public struct ShowPanelRequestEvent : IGameEvent
    {
        public UIPanelType PanelType;

        public ShowPanelRequestEvent(UIPanelType panelType)
        {
            PanelType = panelType;
        }
    }

    public struct LevelButtonPressed : IGameEvent
    {
        public LevelMetadata levelMetadata;

        public LevelButtonPressed(LevelMetadata metadata)
        {
            levelMetadata = metadata;
        }
    }

    

}

