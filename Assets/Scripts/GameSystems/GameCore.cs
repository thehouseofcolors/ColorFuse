using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;


public class GameCore : MonoBehaviour
{
    private readonly HashSet<IGameSystem> _systems = new HashSet<IGameSystem>();

    private async void Awake()
    {
        PlayerPrefs.DeleteAll();
        // await InitialLoader.LoadAsync();
        await Task.CompletedTask;
        // await GameStateMachine.SetInitialStateAsync(new MenuState());
        

    }
    private void Start()
    {
        // Auto-register all systems on the same GameObject
        foreach (var system in GetComponents<IGameSystem>())
        {
            RegisterSystem(system);
        }
    }

    /// <summary>
    /// Register and initialize a system
    /// </summary>
    public void RegisterSystem(IGameSystem system)
    {
        if (system == null || !_systems.Add(system)) return;

        system.Initialize();
        Debug.Log($"[System] Initialized: {system.GetType().Name}");
    }

    /// <summary>
    /// Unregister and shutdown a system
    /// </summary>
    public void UnregisterSystem(IGameSystem system)
    {
        if (system == null || !_systems.Remove(system)) return;

        system.Shutdown();
        Debug.Log($"[System] Shutdown: {system.GetType().Name}");
    }

    private void OnDestroy()
    {
        // Shutdown in reverse registration order
        foreach (var system in _systems)
        {
            system.Shutdown();
            Debug.Log($"[System] Shutdown (OnDestroy): {system.GetType().Name}");
        }

        _systems.Clear();
    }

    /// <summary>
    /// Get a system of specific type
    /// </summary>
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