
public enum AdType
{
    Rewarded,
    Interstitial,
    Banner
}

public static class AdEvents
{
    public struct ShowAdRequestEvent : IGameEvent
    {
        public AdType Type;

        public ShowAdRequestEvent(AdType type)
        {
            Type = type;
        }
    }

    public struct AdStartedEvent : IGameEvent
    {
        public AdType Type;

        public AdStartedEvent(AdType type)
        {
            Type = type;
        }
    }

    public struct AdCompletedEvent : IGameEvent
    {
        public AdType Type;
        public bool WasSuccessful;

        public AdCompletedEvent(AdType type, bool wasSuccessful)
        {
            Type = type;
            WasSuccessful = wasSuccessful;
        }
    }

    public struct AdFailedEvent : IGameEvent
    {
        public AdType Type;
        public string Error;

        public AdFailedEvent(AdType type, string error)
        {
            Type = type;
            Error = error;
        }
    }
}
