using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using LevelEvents;

public class GridManager : MonoBehaviour, IGameSystem
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private float spacing = 1.1f;

    private List<Tile> allTiles = new List<Tile>();

    public void Initialize()
    {
        EventBus.Subscribe<LevelLoadEvent>(OnLevelLoad);
    }

    public void Shutdown()
    {
        
        EventBus.Unsubscribe<LevelLoadEvent>(OnLevelLoad);
    }

    async Task OnLevelLoad(LevelLoadEvent e)
    {
        SetupGrid(e.levelMetadata);
    }

    private void SetupGrid(LevelMetadata level)
    {
        allTiles = GridBuilder.GenerateGrid(gridParent, tilePrefab, spacing, level.levelConfig.gridLength, level.levelConfig.gridWidth);
        var colors = SeedEncoder.DecodeSeedToColors(level.levelConfig.seed);
        PaintManager.PaintTiles(allTiles, colors);
    }
    void OnTileEmptied()
    {
        foreach (var tile in allTiles)
        {
            if (!tile.IsEmpty) return;

        }
        ClearGrid();
        // EventBus.PublishAsync(new );
    }

    public void ClearGrid()
    {
        foreach (var tile in allTiles)
        {
            Destroy(tile.gameObject);
        }
    }


}
