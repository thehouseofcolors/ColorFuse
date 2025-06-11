
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public interface IGameSystem
{
    void Initialize();
    void Shutdown();
}



public class GameCore : MonoBehaviour
{
    private IGameSystem[] gameSystems;

    private void Start()
    {
        gameSystems = GetComponents<IGameSystem>();
        foreach (var system in gameSystems)
        {
            try
            {
                system.Initialize();
            }
            catch(Exception e)
            {
                Debug.LogError($"Error initializing {system.GetType().Name}: {e.Message}");
            }
        }

    }
    private void OnDestroy()
    {
        foreach (var system in gameSystems)
            system.Shutdown();
    }
    public void RegisterSystem(IGameSystem system)
    {
        var systemsList = gameSystems.ToList();
        systemsList.Add(system);
        gameSystems = systemsList.ToArray();
        system.Initialize();
    }

    public void UnregisterSystem(IGameSystem system)
    {
        var systemsList = gameSystems.ToList();
        if (systemsList.Remove(system))
        {
            system.Shutdown();
            gameSystems = systemsList.ToArray();
        }
    }

}
