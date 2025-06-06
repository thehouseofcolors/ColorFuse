public static class UndoEvents
{
    public struct UndoUsageUpdatedEvent : IGameEvent
    {
        public bool CanUndo;
        public UndoUsageUpdatedEvent( bool canUndo)
        {
            CanUndo = canUndo;
        }
    }

    public struct UndoActionRegisteredEvent : IGameEvent
    {
        public int CurrentUndoCount;
        public UndoActionRegisteredEvent(int count)
        {
            CurrentUndoCount = count;
        }
    }

    public struct UndoPerformedEvent : IGameEvent
    {
        public int UndoStepIndex;
        public UndoPerformedEvent(int index)
        {
            UndoStepIndex = index;
        }
    }


    public struct UndoStackClearedEvent : IGameEvent { }

    public struct UndoRequestedEvent : IGameEvent { } //bu button ile tetiklenicek
    
}



