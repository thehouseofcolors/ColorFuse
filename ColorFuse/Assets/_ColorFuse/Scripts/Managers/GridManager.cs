using System.Collections.Generic;
using UnityEngine;

public class TileConfig
{
    public GameObject prefab;
    public Tile tile;
    public Stack<ColorVector> colors;
    public TileConfig(GameObject _prefab, Tile _tile, Stack<ColorVector> _colors)
    {
        prefab = _prefab;
        tile = _tile;
        colors = _colors;
    }
}
public class GridManager : Singleton<GridManager>
{
    [SerializeField] private LevelDatabase levelDatabase;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private float spacing = 1.1f;

    private List<Tile> allTiles = new List<Tile>();
    public int TilesCount => allTiles.Count;

    GridConfig gridConfig;
    private void OnEnable()
    {
        EventBus.Subscribe<LevelStartedEvent>(OnLevelStarted);
        EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<LevelStartedEvent>(OnLevelStarted);
        EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
    }
    
    private void OnLevelStarted(LevelStartedEvent evt)
    {
        SetupGrid(evt.LevelNumber);
    }

    private void OnLevelCompleted(LevelCompletedEvent evt)
    {
        Debug.Log($"Level {evt.TimeRecord} completed!");
    }

    
    // private List<TileConfig> tileConfigs = new List<TileConfig>();

    // void SetupGrid()
    // {
    //     // Örnek: her tile için stack oluşturup TileConfig oluşturma
    //     GameObject tileGO = Instantiate(tilePrefab.gameObject);
    //     Tile tileComponent = tileGO.GetComponent<Tile>();
    //     Stack<ColorVector> colorStack = new Stack<ColorVector>();

    //     TileConfig config = new TileConfig(tileGO, tileComponent, colorStack);
    //     tileConfigs.Add(config);
    // }


    public void SetupGrid(int level)
    {
        gridConfig = levelDatabase.GetGridConfig(level);
        ClearGrid();
        GenerateGrid();
        
        AssignColors(GenerateTileColors(gridConfig.WhiteComboCount));
    }

    private void ClearGrid()
    {
        foreach (var tile in allTiles)
        {
            if (tile != null)
                Destroy(tile.gameObject);
        }
        allTiles.Clear();
    }

    private void GenerateGrid()
    {
        
        float gridWidth = (gridConfig.columns - 1) * spacing;
        float gridHeight = (gridConfig.rows - 1) * spacing;
        Vector2 offset = new Vector2(gridWidth, gridHeight) / 2f;

        for (int x = 0; x < gridConfig.columns; x++)
        {
            for (int y = 0; y < gridConfig.rows; y++)
            {
                Vector2 position = new Vector2(x * spacing, y * spacing) - offset;
                GameObject tileGO = Instantiate(tilePrefab.gameObject, position, Quaternion.identity, transform);
                tileGO.name = $"Tile {x},{y}";

                Tile tile = tileGO.GetComponent<Tile>();
                tile.SetCoordinates(x, y);
                allTiles.Add(tile);
            }
        }
        
        EventBus.Publish(new GridGeneratedEvent());
    }
    List<ColorVector> GenerateTileColors(int size)
    {


        List<ColorVector> allColors = new List<ColorVector>();

        for (int i = 0; i < size; i++)
        {
            List<ColorVector> colorVectors = ColorManager.Instance.GetRandomWhite();
            allColors.AddRange(colorVectors);
            
        }
        foreach (var c in allColors) Debug.Log(c.ToString());
        Shuffle(allColors);
        return allColors;
    }
    private void AssignColors(List<ColorVector> allColors)
    {
        
        int index = 0;
        int stackSizePerTile = allColors.Count / allTiles.Count;

        foreach (var tile in allTiles)
        {
            for (int i = 0; i < stackSizePerTile && index < allColors.Count; i++)
            {
                tile.PushColor(allColors[index++]);
            }
        }
        EventBus.Publish(new ColorsAssignedEvent());
    }

    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }




    public void RedistributeColors()
    {
        List<ColorVector> allColors = new List<ColorVector>();

        foreach (var tile in allTiles)
        {
            while (tile.HasColors())
            {
                allColors.Add(tile.PopTopColor());
            }
        }

        Shuffle(allColors);

        int index = 0;
        int stackSizePerTile = allColors.Count / allTiles.Count;

        foreach (var tile in allTiles)
        {
            for (int i = 0; i < stackSizePerTile && index < allColors.Count; i++)
            {
                tile.PushColor(allColors[index++]);
                tile.UpdateVisual();
            }
        }


    }

    public void CleanupDestroyedTiles()
    {
        allTiles.RemoveAll(tile => tile == null);
    }


}
