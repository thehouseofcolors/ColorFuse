
namespace TimerEvents
{
    public struct TimerStartedEvent : IGameEvent
    {
        public float Duration; // saniye cinsinden

        public TimerStartedEvent(float duration)
        {
            Duration = duration;
        }
    }

    public struct TimerUpdatedEvent : IGameEvent
    {
        public float RemainingTime;

        public TimerUpdatedEvent(float remainingTime)
        {
            RemainingTime = remainingTime;
        }
    }

    public struct TimerFinishedEvent : IGameEvent
    {
    }

    public struct TimerPausedEvent : IGameEvent
    {
    }

    public struct TimerResumedEvent : IGameEvent
    {
    }

    public struct ExtraTimeRequestedEvent : IGameEvent
    {
        public int ExtraSeconds;

        public ExtraTimeRequestedEvent(int seconds)
        {
            ExtraSeconds = seconds;
        }
    }
}
