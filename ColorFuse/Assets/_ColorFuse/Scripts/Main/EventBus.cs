using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGameEvent { }

// Basit async event bus
public static class EventBus
{
    private static Dictionary<Type, List<Func<IGameEvent, Task>>> subscribers 
        = new Dictionary<Type, List<Func<IGameEvent, Task>>>();

    public static void Subscribe<T>(Func<T, Task> handler) where T : IGameEvent
    {
        var type = typeof(T);
        if (!subscribers.TryGetValue(type, out var list))
        {
            list = new List<Func<IGameEvent, Task>>();
            subscribers[type] = list;
        }
        // Cast handler for IGameEvent param
        list.Add((e) => handler((T)e));
    }

 
    public static void Unsubscribe<T>(Func<T, Task> handler) where T : IGameEvent
    {
        var type = typeof(T);
        if (subscribers.TryGetValue(type, out var list))
        {
            list.RemoveAll(h => h.Target == handler.Target && h.Method == handler.Method);
        }
    }

    public static async Task PublishAsync(IGameEvent gameEvent)
    {
        var type = gameEvent.GetType();
        if (subscribers.TryGetValue(type, out var list))
        {
            foreach (var handler in list)
            {
                await handler(gameEvent);
            }
        }
    }
}
