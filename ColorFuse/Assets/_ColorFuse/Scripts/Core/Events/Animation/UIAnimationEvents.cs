
using System;

public enum UIAnimationType
{
    
}
namespace UIAnimationEvents
{

    public struct UIAnimationRequestEvent : IGameEvent
    {
        public UIAnimationType Type;
        public Action OnComplete;

        public UIAnimationRequestEvent(UIAnimationType type, Action onComplete = null)
        {
            Type = type;
            OnComplete = onComplete;
        }
    }


}



