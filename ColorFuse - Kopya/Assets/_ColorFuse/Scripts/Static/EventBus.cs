using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Subscription>> _subscribers = new();
    private static readonly object _lock = new();

    public static IDisposable Subscribe<T>(Func<T, Task> handler) where T : IGameEvent
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var subscription = new Subscription<T>(handler);

        lock (_lock)
        {
            if (!_subscribers.TryGetValue(typeof(T), out var subscriptions))
            {
                subscriptions = new List<Subscription>();
                _subscribers[typeof(T)] = subscriptions;
            }
            subscriptions.Add(subscription);
        }

        return new DisposableToken(() => Unsubscribe(subscription));
    }
    public static IDisposable Subscribe<T>(Action<T> handler) where T : IGameEvent
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var subscription = new SyncSubscription<T>(handler);

        lock (_lock)
        {
            if (!_subscribers.TryGetValue(typeof(T), out var subscriptions))
            {
                subscriptions = new List<Subscription>();
                _subscribers[typeof(T)] = subscriptions;
            }
            subscriptions.Add(subscription);
        }

        return new DisposableToken(() => Unsubscribe(subscription));
    }
    private class SyncSubscription<T> : Subscription where T : IGameEvent
    {
        private readonly Action<T> _handler;
        public override Type EventType => typeof(T);

        public SyncSubscription(Action<T> handler) => _handler = handler;

        public override Task Handle(IGameEvent gameEvent)
        {
            _handler((T)gameEvent);
            return Task.CompletedTask;
        }
    }

    private static void Unsubscribe(Subscription subscription)
    {
        lock (_lock)
        {
            if (_subscribers.TryGetValue(subscription.EventType, out var subscriptions))
            {
                subscriptions.Remove(subscription);
                if (subscriptions.Count == 0)
                {
                    _subscribers.Remove(subscription.EventType);
                }
            }
        }
    }

    public static async Task PublishAsync<T>(T gameEvent) where T : IGameEvent
    {
        if (gameEvent == null) throw new ArgumentNullException(nameof(gameEvent));

        Subscription[] subscriptionsCopy;
        lock (_lock)
        {
            if (!_subscribers.TryGetValue(typeof(T), out var subscriptions))
                return;

            subscriptionsCopy = subscriptions.ToArray();
        }

        foreach (var subscription in subscriptionsCopy)
        {
            try
            {
                await subscription.Handle(gameEvent);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[EventBus] async handler error: {ex}");
            }
        }
    }

    public static void PublishSync<T>(T gameEvent) where T : IGameEvent
    {
        if (gameEvent == null) throw new ArgumentNullException(nameof(gameEvent));

        Subscription[] subscriptionsCopy;
        lock (_lock)
        {
            if (!_subscribers.TryGetValue(typeof(T), out var subscriptions))
                return;

            subscriptionsCopy = subscriptions.ToArray();
        }

        foreach (var subscription in subscriptionsCopy)
        {
            try
            {
                subscription.Handle(gameEvent).GetAwaiter().GetResult(); // sync olarak çalıştır
            }
            catch (Exception ex)
            {
                Debug.LogError($"[EventBus] sync handler error: {ex}");
            }
        }
    }

    public static void Clear()
    {
        lock (_lock)
        {
            _subscribers.Clear();
        }
    }

    #region Internal Types
    private abstract class Subscription
    {
        public abstract Type EventType { get; }
        public abstract Task Handle(IGameEvent gameEvent);
    }

    private class Subscription<T> : Subscription where T : IGameEvent
    {
        private readonly Func<T, Task> _handler;
        public override Type EventType => typeof(T);

        public Subscription(Func<T, Task> handler) => _handler = handler;

        public override Task Handle(IGameEvent gameEvent) => _handler((T)gameEvent);
    }

    private class DisposableToken : IDisposable
    {
        private readonly Action _onDispose;

        public DisposableToken(Action onDispose) => _onDispose = onDispose;
        public void Dispose() => _onDispose?.Invoke();
    }
    #endregion
}

