using System;
using System.Collections.Generic;


public interface IGameEvent { }



public static class EventBus
{
    private static Dictionary<Type, List<Delegate>> _subscribers = new();

    public static void Subscribe<T>(Action<T> listener) where T : IGameEvent
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();

        _subscribers[type].Add(listener);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : IGameEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            list.Remove(listener);
        }
    }

    public static void Publish<T>(T gameEvent) where T : IGameEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            foreach (var listener in list)
                (listener as Action<T>)?.Invoke(gameEvent);
        }
    }

    public static void ClearAll() => _subscribers.Clear();
}

