using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class LevelManager : Singleton<LevelManager>, IGameSystem
{
    private GameObject _currentLevelInstance;
    private LevelConfig _levelConfig;
    private int _preloadedLevelNumber = -1;
    private AsyncOperationHandle<GameObject> _preloadedLevelHandle;
    public LevelConfig GetLevelConfig => _levelConfig;
    public async Task PreloadLevel(int levelNumber)
    {
        if (_preloadedLevelNumber == levelNumber && _preloadedLevelHandle.IsValid())
        {
            Debug.Log($"Level {levelNumber} zaten preload edilmiş.");
            return;
        }

        if (_preloadedLevelHandle.IsValid())
        {
            Addressables.Release(_preloadedLevelHandle);
            _preloadedLevelHandle = default;
            _preloadedLevelNumber = -1;
        }

        string address = $"Level_{levelNumber}";
        _preloadedLevelHandle = Addressables.LoadAssetAsync<GameObject>(address);
        await _preloadedLevelHandle.Task;

        if (_preloadedLevelHandle.Status == AsyncOperationStatus.Succeeded)
        {
            _preloadedLevelNumber = levelNumber;
            Debug.Log($"Level {levelNumber} preload başarılı.");
        }
        else
        {
            Debug.LogError($"Level {levelNumber} preload başarısız.");
        }
    }

    public GameObject InstantiatePreloadedLevel()
    {
        if (_preloadedLevelHandle.IsValid() && _preloadedLevelHandle.Status == AsyncOperationStatus.Succeeded)
        {
            return GameObject.Instantiate(_preloadedLevelHandle.Result);
        }

        Debug.LogWarning("Level önceden preload edilmemiş veya yükleme başarısız!");
        return null;
    }

    public void UnloadCurrentLevel()
    {
        if (_currentLevelInstance != null)
        {
            GameObject.Destroy(_currentLevelInstance);
            _currentLevelInstance = null;
        }
    }

    public void Initialize() { }
    public void Shutdown()
    {
    }
}


