
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.IO;
using GameEvents;
using System.Linq;
using System.Threading;


#region  Interfaces

public interface IGameEvent { }

public interface IGameSystem
{
    void Initialize();
    void Shutdown();
}
public interface IGameState
{
    Task EnterAsync();   // Duruma girerken async işlemler olabilir
    Task ExitAsync();    // Durumdan çıkarken async işlemler
}

#endregion


#region  Color-System
public struct ColorVector
{
    public int R, G, B;
    public bool IsNull;  // New field to represent null state

    public static readonly ColorVector Null = new ColorVector { IsNull = true };

    public ColorVector(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
        IsNull = false;
    }

    // Add this new property
    public bool HasValue => !IsNull;

    // Modify your existing methods to handle null case
    public Color ToUnityColor()
    {
        return IsNull ? Color.clear : new Color(R, G, B);
    }

    // Update operator overloads to handle null cases
    public static ColorVector operator +(ColorVector a, ColorVector b)
    {
        if (a.IsNull || b.IsNull) return Null;
        return new ColorVector(a.R + b.R, a.G + b.G, a.B + b.B);
    }

    public static bool operator ==(ColorVector a, ColorVector b)
    {
        return a.R == b.R && a.G == b.G && a.B == b.B;
    }

    public static bool operator !=(ColorVector a, ColorVector b)
    {
        return !(a == b);
    }

    // Similarly update other operators and properties
    public bool IsWhite => !IsNull && R == 1 && G == 1 && B == 1;

    public bool IsValidColor => !IsNull &&
        (R == 0 || R == 1) && (G == 0 || G == 1) && (B == 0 || B == 1)
        && (R + G + B > 0); // Siyah değil
    public bool IsBaseColor =>
        (R + G + B == 1) && IsValidColor;
    public bool IsIntermediateColor =>
        (R + G + B == 2) && IsValidColor;


    public override string ToString() =>
        IsNull ? "Null" : $"({R}, {G}, {B})";

    public bool Equals(ColorVector other) => this == other;

    public override bool Equals(object obj) =>
        obj is ColorVector other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(R, G, B);
}


// [System.Serializable]
public struct ColorRecipe  // More descriptive than "Combo"
{
    public List<ColorVector> components;  // Better than "parts"

    public ColorRecipe(params ColorVector[] components)
    {
        this.components = new List<ColorVector>(components);
    }
}
// public readonly struct ColorRecipe : IEnumerable<ColorVector>
// {
//     private readonly ColorVector[] _components;
    
//     public ColorRecipe(params ColorVector[] components)
//     {
//         if (components == null || components.Length == 0)
//             throw new ArgumentException("Components cannot be null or empty");
            
//         _components = components;
//     }
    
//     public IEnumerator<ColorVector> GetEnumerator() => 
//         ((IEnumerable<ColorVector>)_components).GetEnumerator();
        
//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
// }

public static class WhiteRecipes
{
    private static readonly IReadOnlyList<ColorRecipe> _recipes = new List<ColorRecipe>
    {
        new ColorRecipe(new ColorVector(1,0,0), new ColorVector(0,1,0), new ColorVector(0,0,1)),
        new ColorRecipe(new ColorVector(1,1,0), new ColorVector(0,0,1)),
        new ColorRecipe(new ColorVector(0,1,1), new ColorVector(1,0,0)),
        new ColorRecipe(new ColorVector(1,0,1), new ColorVector(0,1,0))
    }.AsReadOnly();

    public static IReadOnlyList<ColorRecipe> AllRecipes => _recipes;

    public static ColorRecipe GetRandomRecipe(System.Random rng = null)
    {
        return rng != null
            ? _recipes[rng.Next(_recipes.Count)]
            : _recipes[UnityEngine.Random.Range(0, _recipes.Count)];
    }
    public static ColorRecipe GetRandomRecipe()
    {
        return _recipes[UnityEngine.Random.Range(0, _recipes.Count)];
    }
}
public static class ColorFusion
{
    /// <summary>
    /// Checks if two colors can be fused
    /// </summary>
    public static bool CanFuse(ColorVector a, ColorVector b)
    {
        if (a.IsNull || b.IsNull) return false;
        if (a == b) return false;
        
        var merged = a + b;
        return merged.IsValidColor;
    }

    public static ColorVector Fuse(ColorVector a, ColorVector b)
    {
        if (CanFuse(a, b))
        {
            return new ColorVector(
                Mathf.Clamp(a.R + b.R, 0, 1),
                Mathf.Clamp(a.G + b.G, 0, 1),
                Mathf.Clamp(a.B + b.B, 0, 1)
            );
        }
        return new ColorVector(0, 0, 0); // ya da ColorVector.Invalid


    }
}

public static class ColorPalette
{
    private static readonly ColorVector[] palette = new ColorVector[]
    {
        new ColorVector(1, 0, 0),
        new ColorVector(0, 1, 0),
        new ColorVector(0, 0, 1),
        new ColorVector(1, 1, 0),
        new ColorVector(0, 1, 1),
        new ColorVector(1, 0, 1)
    };

    public static ColorVector GetColorVector(int index) => palette[index];
    public static int GetColorIndex(ColorVector vector) => Array.IndexOf(palette, vector);



}

#endregion


#region Grid-System

public static class PaintManager
{
    public static async Task PaintTiles(List<Tile> tiles, Stack<ColorVector> colors,
        CancellationToken cancellationToken = default)
    {
        if (tiles == null) throw new ArgumentNullException(nameof(tiles));
        if (colors == null) throw new ArgumentNullException(nameof(colors));

        while (colors.Count > 0 && !cancellationToken.IsCancellationRequested)
        {
            foreach (var tile in tiles)
            {
                if (colors.Count == 0 || cancellationToken.IsCancellationRequested)
                    return;

                var color = colors.Pop();

                // Use MainThreadDispatcher if available, otherwise execute directly
                if (MainThreadDispatcher.Instance)
                {
                    await MainThreadDispatcher.Instance.EnqueueAsync(() =>
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            tile.PushColor(color);
                            tile.UpdateVisual();
                        }
                    }, cancellationToken);
                }
                else
                {
                    // Fallback for when dispatcher isn't available
                    tile.PushColor(color);
                    tile.UpdateVisual();
                }

                await Task.Delay(300, cancellationToken).ConfigureAwait(true); // Force main thread continuation
            }
        }
    }
}


public static class GridBuilder
{
    public static List<Tile> GenerateGrid(Transform parent, Tile tilePrefab, float spacing, int rows, int columns)
    {
        List<Tile> tiles = new List<Tile>();

        #region nullChecks

        if (parent == null)
        {
            Debug.LogError("GridBuilder: Parent transform is null!");
            return tiles;
        }

        if (tilePrefab == null)
        {
            Debug.LogError("GridBuilder: Tile prefab is null!");
            return tiles;
        }

        if (columns <= 0 || rows <= 0)
        {
            Debug.LogWarning($"GridBuilder: Invalid grid size ({columns}x{rows})");
            return tiles;
        }


        #endregion



        float gridWidth = (columns - 1) * spacing;
        float gridHeight = (rows - 1) * spacing;
        Vector2 offset = new Vector2(gridWidth, gridHeight) / 2f;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 position = new Vector2(x * spacing, y * spacing) - offset;

                GameObject tileGO = UnityEngine.Object.Instantiate(tilePrefab.gameObject, position, Quaternion.identity, parent);
                if (tileGO == null)
                {
                    Debug.LogError($"GridBuilder: Failed to instantiate tile at ({x},{y})");
                    continue;
                }

                tileGO.name = $"Tile {x},{y}";
                var tile = tileGO.GetComponent<Tile>();

                if (tile == null)
                {
                    Debug.LogError($"GridBuilder: Instantiated object at ({x},{y}) does not have a Tile component.");
                    UnityEngine.Object.Destroy(tileGO);
                    continue;
                }

                tile.SetCoordinates(x, y);tile.CanSelectable = false;
                tiles.Add(tile);
            }
        }

        Debug.Log($"GridBuilder: Generated {tiles.Count} tiles.");
        return tiles;
    }

    
}


#endregion


#region  seed-System
public static class SeedGenerator
{
    public static List<ColorVector> GenerateAllTileColors(int whiteComboCount)
    {
        var colors = new List<ColorVector>();
        for (int i = 0; i < whiteComboCount; i++)
        {
            colors.AddRange(WhiteRecipes.GetRandomRecipe().components);
        }
        colors.Shuffle();
        return colors;
    }

    public static string GenerateSeed(int whiteComboCount)
    {
        var colors = GenerateAllTileColors(whiteComboCount);

        return SeedEncoder.EncodeColorsToSeed(colors);
    }
}

public static class SeedEncoder
{
    public static string EncodeColorsToSeed(List<ColorVector> colorVectors)
    {
        var builder = new System.Text.StringBuilder();
        foreach (var c in colorVectors)
        {
            builder.Append(ColorPalette.GetColorIndex(c));
        }
        return builder.ToString();
    }


    public static Stack<ColorVector> DecodeSeedToColors(string seed)
    {
        var colorVectors = new Stack<ColorVector>();
        foreach (char ch in seed)
        {
            int index = int.Parse(ch.ToString());
            ColorVector vector = ColorPalette.GetColorVector(index);
            colorVectors.Push(vector);
        }
        return colorVectors;
    }

}
#endregion


#region Game-services

// public static class GamePaths
// {
//     public const string StreamingLevelsFolder = "Levels";
//     public const string LevelsFileName = "levels.json";

//     public static string LevelsJsonPath =>
//         Path.Combine(Application.streamingAssetsPath, StreamingLevelsFolder, LevelsFileName);

// #if UNITY_EDITOR
//     public static string EditorLevelsJsonPath =>
//         Path.Combine(Application.dataPath, "StreamingAssets", StreamingLevelsFolder, LevelsFileName);
// #endif
// }

// public static class InitialLoader
// {
//     public static async Task LoadAsync()
//     {
//         Debug.Log("InitialLoader başladı");

//         await EventBus.PublishAsync(new ScreenChangeEvent(ScreenType.Loading));

//         // Verileri yükle
//         LevelDataManager.AllLevels = (await LevelDataLoader.LoadAllLevelConfigsAsync()).ToList();

//         // Butonları oluştur
//         LevelMenuPanel.Instance.CreateLevelButtons(LevelDataManager.AllLevels);

//         Debug.Log("InitialLoader tamamlandı");
//     }
// }

public static class Constants
{
    public static readonly string CurrentLevelKey = "CurrentLevel";
    public static readonly string RemainingUndoKey = "Undo";
    public static readonly string RemainingMovesleKey = "Moves";
    public static readonly string TimerStartKey = "Timer";
    public static readonly string SoundOn = "SoundEnabled";
    public static readonly string MusicOn = "MusicEnabled";


}

public static class PlayerPrefsService
{
    // Seviye Bilgileri
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(Constants.CurrentLevelKey, 1);
        set => PlayerPrefs.SetInt(Constants.CurrentLevelKey, value);
    }
    public static int TimerStart
    {
        get => PlayerPrefs.GetInt(Constants.TimerStartKey, 10);
        set => PlayerPrefs.SetInt(Constants.TimerStartKey, value);
    }

    public static int RemainingUndo
    {
        get => PlayerPrefs.GetInt(Constants.RemainingUndoKey, 5);
        set => PlayerPrefs.SetInt(Constants.RemainingUndoKey, value);
    }

    public static int RemainingMoves
    {
        get => PlayerPrefs.GetInt(Constants.RemainingMovesleKey, 20);
        set => PlayerPrefs.SetInt(Constants.RemainingMovesleKey, value);
    }

    // Ayarlar
    public static bool IsSoundOn
    {
        get => PlayerPrefs.GetInt(Constants.SoundOn, 1) == 1;
        set => PlayerPrefs.SetInt(Constants.SoundOn, value ? 1 : 0);
    }

    public static bool IsMusicOn
    {
        get => PlayerPrefs.GetInt(Constants.MusicOn, 1) == 1;
        set => PlayerPrefs.SetInt(Constants.MusicOn, value ? 1 : 0);
    }

    // Ortak Kullanımlar
    public static void Save() => PlayerPrefs.Save();

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

#endregion

#region  Joker System
public struct JokerConfig
{
    public int time;
    public int moves;
    public JokerConfig(int _time, int _moves)
    {
        time = _time;
        moves = _moves;
    }
}

#endregion

#region  Level System

[System.Serializable]
public class LevelConfig
{
    public int level;
    public int tiles_in_a_row;
    public int tiles_in_a_column;
    public int targetWhiteTiles;
    public float timeLimit;
    public int moveLimit;
    public string seed;
}

public static class LevelDataManager
{
    [SerializeField]
    public static List<LevelConfig> AllLevels;
    

    public static LevelConfig GetLevelConfig(int levelNumber)
    {
        return AllLevels?.Find(l => l.level == levelNumber);
    }
}

// public static class LevelDataLoader
// {

//     // [Serializable]
//     // private class LevelConfigWrapper
//     // {
//     //     public LevelConfig[] levels;
//     // }

//     // public static async Task<LevelConfig> LoadLevelConfigAsync(int levelNumber)
//     // {
//     //     try
//     //     {
//     //         var allLevels = await LoadAllLevelConfigsAsync();
//     //         return Array.Find(allLevels, config => config.level == levelNumber)
//     //                ?? CreateDefaultLevel(levelNumber);
//     //     }
//     //     catch (Exception e)
//     //     {
//     //         Debug.LogError($"Failed to load level {levelNumber}: {e.Message}");
//     //         return CreateDefaultLevel(levelNumber);
//     //     }
//     // }

//     //     public static async Task<LevelConfig[]> LoadAllLevelConfigsAsync()
//     //     {
//     //         string filePath = GamePaths.LevelsJsonPath;

//     //         try
//     //         {
//     // #if UNITY_ANDROID || UNITY_WEBGL
//     //             return await LoadViaUnityWebRequest(filePath);
//     // #else
//     //             return await LoadViaFileSystem(filePath);
//     // #endif
//     //         }
//     //         catch (Exception e)
//     //         {
//     //             Debug.LogError($"Level loading error: {e.Message}");
//     //             return new[] { CreateDefaultLevel(1) };
//     //         }
//     //     }

//     // private static async Task<LevelConfig[]> LoadViaUnityWebRequest(string path)
//     // {
//     //     using (var www = new UnityEngine.Networking.UnityWebRequest(path))
//     //     {
//     //         www.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
//     //         var operation = www.SendWebRequest();

//     //         while (!operation.isDone)
//     //             await Task.Yield();

//     //         if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
//     //             throw new Exception(www.error);

//     //         var wrapper = JsonUtility.FromJson<LevelConfigWrapper>(www.downloadHandler.text);
//     //         return wrapper?.levels ?? throw new Exception("Invalid level data format");
//     //     }
//     // }

//     // private static async Task<LevelConfig[]> LoadViaFileSystem(string path)
//     // {
//     //     if (!File.Exists(path))
//     //         throw new FileNotFoundException("Level config file not found");

//     //     string json = await File.ReadAllTextAsync(path);
//     //     var wrapper = JsonUtility.FromJson<LevelConfigWrapper>(json);
//     //     return wrapper?.levels ?? throw new Exception("Invalid level data format");
//     // }

//     // private static LevelConfig CreateDefaultLevel(int levelNumber)
//     // {
//     //     return new LevelConfig
//     //     {
//     //         level = levelNumber,
//     //         tiles_in_a_column = 5,
//     //         tiles_in_a_row = 5,
//     //         targetWhiteTiles = 3,
//     //         timeLimit = 60f,
//     //         moveLimit = 20,
//     //         seed = "default_" + levelNumber
//     //     };
//     // }

// }

#endregion


#region  Extensions
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
    public static void Shuffle<T>(this IList<T> list, System.Random rng)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public static T PickRandom<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
            throw new System.InvalidOperationException("Liste boş!");

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || list.Count == 0;
    }

}

public static class TaskExtensions
{
    public static void Forget(this Task task)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogException(t.Exception);
        }, TaskContinuationOptions.OnlyOnFaulted);
    }
}

#endregion