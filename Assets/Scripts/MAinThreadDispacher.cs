using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static MainThreadDispatcher _instance;

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (_instance == null)
            {
                Initialize();
            }
            return _instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_instance == null)
        {
            var obj = new GameObject("MainThreadDispatcher");
            _instance = obj.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(obj);
        }
    }

    public void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

        // Async uyumlu versiyon
    public Task EnqueueAsync(Action action, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled(cancellationToken);

        var tcs = new TaskCompletionSource<bool>();
        
        Enqueue(() => 
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return;
            }

            try
            {
                action();
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        
        return tcs.Task;
    }
}