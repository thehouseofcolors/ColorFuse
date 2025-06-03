
using UnityEngine;
using System.Collections.Generic;
using System.Linq;  

public interface IGameSystem
{
    void Initialize();
    void Shutdown(); // Gerekirse kullanılabilir
}



public class GameCore : MonoBehaviour
{
    private IGameSystem[] gameSystems;

    private void Awake()
    {
        gameSystems = GetComponents<IGameSystem>();
        foreach (var system in gameSystems)
            system.Initialize();
    }
    void Start()
    {

    }
    private void OnDestroy()
    {
        foreach (var system in gameSystems)
            system.Shutdown();
    }
}
