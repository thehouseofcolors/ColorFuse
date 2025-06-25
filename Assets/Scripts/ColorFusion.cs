
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

public interface IPanel 
{
    Task ShowAsync(object transitionData);
    Task HideAsync();
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
    #region Game Progress
    public const string CurrentLevelKey = "CurrentLevel";
    public const string HighestLevelKey = "HighestLevel";
    #endregion

    #region Game Session
    public const string RemainingUndoKey = "Undo";
    public const string RemainingMovesKey = "Moves";
    public const string TimerStartKey = "Timer";
    #endregion

    #region Settings
    public const string SoundEnabledKey = "SoundEnabled";
    public const string MusicEnabledKey = "MusicEnabled";
    public const string VibrationEnabledKey = "VibrationEnabled";
    public const string LanguageKey = "Language";
    #endregion

    #region Player Stats
    public const string TotalScoreKey = "TotalScore";
    public const string LevelsCompletedKey = "LevelsCompleted";
    #endregion
}

public static class PlayerPrefsService
{
    #region Game Progress
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(Constants.CurrentLevelKey, 1);
        set
        {
            PlayerPrefs.SetInt(Constants.CurrentLevelKey, value);
            if (value > HighestLevel)
            {
                HighestLevel = value;
            }
        }
    }

    public static int HighestLevel
    {
        get => PlayerPrefs.GetInt(Constants.HighestLevelKey, 1);
        private set => PlayerPrefs.SetInt(Constants.HighestLevelKey, value);
    }

    #endregion

    #region Game Session
    public static int RemainingUndo
    {
        get => PlayerPrefs.GetInt(Constants.RemainingUndoKey, 5);
        set => PlayerPrefs.SetInt(Constants.RemainingUndoKey, Mathf.Max(0, value));
    }

    public static int RemainingMoves
    {
        get => PlayerPrefs.GetInt(Constants.RemainingMovesKey, 20);
        set => PlayerPrefs.SetInt(Constants.RemainingMovesKey, Mathf.Max(0, value));
    }

    public static int TimerStart
    {
        get => PlayerPrefs.GetInt(Constants.TimerStartKey, 60);
        set => PlayerPrefs.SetInt(Constants.TimerStartKey, Mathf.Max(0, value));
    }
    #endregion

    #region Settings
    public static bool IsSoundOn
    {
        get => PlayerPrefs.GetInt(Constants.SoundEnabledKey, 1) == 1;
        set => PlayerPrefs.SetInt(Constants.SoundEnabledKey, value ? 1 : 0);
    }

    public static bool IsMusicOn
    {
        get => PlayerPrefs.GetInt(Constants.MusicEnabledKey, 1) == 1;
        set => PlayerPrefs.SetInt(Constants.MusicEnabledKey, value ? 1 : 0);
    }

    public static bool IsVibrationOn
    {
        get => PlayerPrefs.GetInt(Constants.VibrationEnabledKey, 1) == 1;
        set => PlayerPrefs.SetInt(Constants.VibrationEnabledKey, value ? 1 : 0);
    }

    public static string Language
    {
        get => PlayerPrefs.GetString(Constants.LanguageKey, "en");
        set => PlayerPrefs.SetString(Constants.LanguageKey, value);
    }
    #endregion

    #region Player Stats
    public static int TotalScore
    {
        get => PlayerPrefs.GetInt(Constants.TotalScoreKey, 0);
        set => PlayerPrefs.SetInt(Constants.TotalScoreKey, Mathf.Max(0, value));
    }

    public static int LevelsCompleted
    {
        get => PlayerPrefs.GetInt(Constants.LevelsCompletedKey, 0);
        set => PlayerPrefs.SetInt(Constants.LevelsCompletedKey, Mathf.Max(0, value));
    }
    #endregion

    #region Utility Methods
    public static void Save() => PlayerPrefs.Save();

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(Constants.CurrentLevelKey);
        PlayerPrefs.DeleteKey(Constants.HighestLevelKey);
        PlayerPrefs.DeleteKey(Constants.TotalScoreKey);
        PlayerPrefs.DeleteKey(Constants.LevelsCompletedKey);
        Save();
    }

    public static void ResetSettings()
    {
        PlayerPrefs.DeleteKey(Constants.SoundEnabledKey);
        PlayerPrefs.DeleteKey(Constants.MusicEnabledKey);
        PlayerPrefs.DeleteKey(Constants.VibrationEnabledKey);
        PlayerPrefs.DeleteKey(Constants.LanguageKey);
        Save();
    }

    public static void IncrementLevel()
    {
        CurrentLevel++;
    }

    public static void AddScore(int score)
    {
        TotalScore += score;
        if (score > 0)
        {
            LevelsCompleted++;
        }
    }
    #endregion
}
#endregion

// #region  Joker System
// public struct JokerConfig
// {
//     public int time;
//     public int moves;
//     public JokerConfig(int _time, int _moves)
//     {
//         time = _time;
//         moves = _moves;
//     }
// }

// #endregion


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