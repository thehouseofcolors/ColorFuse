public static class ShuffleEvents
{
    public class ShuffleRequestedEvent : IGameEvent { } //bu buttonla tetiklencek

    public class ShuffledEvent : IGameEvent
    {
        public int RemainingShuffles;
        public ShuffledEvent(int remaining) => RemainingShuffles = remaining;
    }


    public class ShuffleUsageUpdatedEvent : IGameEvent
    {
        public bool CanShuffle;
        public ShuffleUsageUpdatedEvent(bool canShuffle)
        {
            CanShuffle = canShuffle;
        }
    }

}
