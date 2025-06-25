using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;


public class GameCore : MonoBehaviour
{
    private readonly HashSet<IGameSystem> _systems = new HashSet<IGameSystem>();

    private void Start()
    {
        foreach (var system in GetComponents<IGameSystem>())
        {
            RegisterSystem(system);
        }
    }
    public void RegisterSystem(IGameSystem system)
    {
        if (system == null || !_systems.Add(system)) return;

        system.Initialize();
        Debug.Log($"[System] Initialized: {system.GetType().Name}");
    }

    public void UnregisterSystem(IGameSystem system)
    {
        if (system == null || !_systems.Remove(system)) return;

        system.Shutdown();
        Debug.Log($"[System] Shutdown: {system.GetType().Name}");
    }

    private void OnDestroy()
    {
        foreach (var system in _systems)
        {
            system.Shutdown();
            Debug.Log($"[System] Shutdown (OnDestroy): {system.GetType().Name}");
        }

        _systems.Clear();
    }

    public T GetSystem<T>() where T : class, IGameSystem
    {
        foreach (var system in _systems)
        {
            if (system is T typedSystem)
            {
                return typedSystem;
            }
        }
        return null;
    }
    


}