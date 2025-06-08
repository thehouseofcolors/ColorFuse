using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GridBuilder
{
    public static List<Tile> GenerateGrid(Transform parent, Tile tilePrefab, float spacing, int columns, int rows)
    {
        List<Tile> tiles = new List<Tile>();

        // Null kontrolleri
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

        float gridWidth = (columns - 1) * spacing;
        float gridHeight = (rows - 1) * spacing;
        Vector2 offset = new Vector2(gridWidth, gridHeight) / 2f;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 position = new Vector2(x * spacing, y * spacing) - offset;

                GameObject tileGO = Object.Instantiate(tilePrefab.gameObject, position, Quaternion.identity, parent);
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
                    Object.Destroy(tileGO);
                    continue;
                }

                tile.SetCoordinates(x, y);
                tiles.Add(tile);
            }
        }

        Debug.Log($"GridBuilder: Generated {tiles.Count} tiles.");
        return tiles;
    }

    
}

public static class TileColorDistributor
{
    public static void DistributeColors(List<Tile> tiles, Stack<ColorVector> colors)
    {
        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogWarning("TileColorDistributor: tiles list is null veya boş!");
            return;
        }

        if (colors == null || colors.Count == 0)
        {
            Debug.LogWarning("TileColorDistributor: renk yığını boş veya null!");
            return;
        }

        while (colors.Count > 0)
        {
            foreach (var tile in tiles)
            {
                if (!colors.TryPop(out ColorVector result))
                    return; // renk kalmadıysa metottan çık

                tile.PushColor(result);
            }
        }
    }
    
}


public class GridManager : MonoBehaviour, IGameSystem
{
    [SerializeField] private LevelDatabase levelDatabase;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private float spacing = 1.1f;

    private List<Tile> allTiles = new List<Tile>();
    private LevelProgressChecker levelProgressChecker;

    public void Initialize()
    {
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<TileEvents.TileEmptiedEvent>(_ => levelProgressChecker?.OnTileEmptied());
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<TileEvents.TileEmptiedEvent>(_ => levelProgressChecker?.OnTileEmptied());
    }

    private void OnLevelStarted(LevelEvents.LevelStartedEvent evt)
    {
        if (!evt.LevelConfig.isRestarted)
        {
            SetupGrid(evt.LevelConfig.level);
        }
    }

    private void SetupGrid(int level)
    {
        if (!levelDatabase.TryGetGridConfig(level, out var gridConfig)) return;

        // Eğer grid zaten kuruluysa yeniden kurma
        if (allTiles.Count == gridConfig.columns * gridConfig.rows)
        {
            Stack<ColorVector> newColors = ColorManager.GenerateColors(gridConfig.WhiteComboCount);
            TileColorDistributor.DistributeColors(allTiles, newColors);
        }
        else
        {
            // Eski davranış: yeniden kur
            ClearGrid(); // Hâlâ Destroy ediyor
            allTiles = GridBuilder.GenerateGrid(gridParent, tilePrefab, spacing, gridConfig.columns, gridConfig.rows);
            Stack<ColorVector> allColors = ColorManager.GenerateColors(gridConfig.WhiteComboCount);
            TileColorDistributor.DistributeColors(allTiles, allColors);
        }

        levelProgressChecker = new LevelProgressChecker(allTiles);
    }

    public void ClearGrid()
    {
        foreach (var tile in allTiles)
        {
            Destroy(tile.gameObject);
        }
    }
    public IEnumerator ClearGridAnimated()
    {
        foreach (var tile in allTiles)
        {
            if (tile != null)
                yield return StartCoroutine(tile.PlayClearColorAnimation(0.2f));
        }

        Debug.Log("[GridManager] Tüm tile içerikleri temizlendi (objeler kaldı).");
    }

}
