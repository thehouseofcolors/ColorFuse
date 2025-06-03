using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour, IGameSystem
{
    [SerializeField] private LevelDatabase levelDatabase;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] Transform gridParent;
    [SerializeField] private float spacing = 1.1f;

    private List<Tile> allTiles = new List<Tile>();
    private List<ColorVector> colors = new List<ColorVector>();
    private GridConfig gridConfig;

    public void Initialize()
    {
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }

    private void OnLevelStarted(LevelEvents.LevelStartedEvent evt)
    {
        if (!evt.IsRestart)
        {
            SetupGrid(evt.Level);
        }
        else
        {
            Debug.Log("Level restart detected. Grid setup skipped.");
        }
    }

    private void SetupGrid(int level)
    {
        if (levelDatabase == null)
        {
            Debug.LogError("GridManager: LevelDatabase is null!");
            return;
        }

        if (!levelDatabase.TryGetGridConfig(level, out gridConfig))
        {
            Debug.LogError($"GridManager: Level {level} grid config not found! Cannot generate grid.");
            return;
        }


        if (tilePrefab == null)
        {
            Debug.LogError("GridManager: Tile prefab is not assigned!");
            return;
        }

        GridBuilder.ClearGrid(allTiles);

        allTiles = GridBuilder.GenerateGrid(gridParent, tilePrefab, spacing, gridConfig.columns, gridConfig.rows);
        Debug.Log($"GridManager: Generated {allTiles.Count} tiles for level {level}");

        if (allTiles.Count == 0)
        {
            Debug.LogWarning("GridManager: No tiles were generated. Aborting color distribution.");
            return;
        }

        colors = TileColorDistributor.GenerateColors(gridConfig.WhiteComboCount);
        if (colors == null || colors.Count == 0)
        {
            Debug.LogWarning("GridManager: Generated color list is null or empty.");
        }

        TileColorDistributor.DistributeColors(allTiles, colors);
        Debug.Log("GridManager: Color distribution completed.");
    }


}
