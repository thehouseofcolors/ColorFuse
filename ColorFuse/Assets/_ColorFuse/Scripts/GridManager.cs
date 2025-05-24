using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private float spacing = 1.1f;

    private List<Tile> allTiles = new List<Tile>();

    private int width;
    private int height;
    private int stackSizePerTile;
    private int baseColorCount;
    private int mixedColorCount;

    public void SetupGrid(GridConfig config)
    {
        width = config.columns;
        height = config.rows;
        baseColorCount = config.baseColorCount;
        mixedColorCount = config.mixedColorCount;
        stackSizePerTile = baseColorCount + mixedColorCount;

        ClearGrid();
        GenerateGrid();
        GenerateAndAssignColors();
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
        float gridWidth = (width - 1) * spacing;
        float gridHeight = (height - 1) * spacing;
        Vector2 offset = new Vector2(gridWidth, gridHeight) / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x * spacing, y * spacing) - offset;
                GameObject tileGO = Instantiate(tilePrefab.gameObject, position, Quaternion.identity, transform);
                tileGO.name = $"Tile {x},{y}";

                Tile tile = tileGO.GetComponent<Tile>();
                tile.SetCoordinates(x, y);
                allTiles.Add(tile);
            }
        }
    }

    private void GenerateAndAssignColors()
    {
        int totalTileCount = width * height;
        int totalColorCount = totalTileCount * stackSizePerTile;

        List<ColorVector> allColors = new List<ColorVector>();

        // baseColorCount kadar ana renk dizisi oluştur
        ColorVector[] baseColors = GenerateBaseColors(baseColorCount);

        int countPerColor = totalColorCount / baseColors.Length;

        for (int i = 0; i < baseColors.Length; i++)
        {
            for (int j = 0; j < countPerColor; j++)
            {
                allColors.Add(baseColors[i]);
            }
        }

        // Eksik renkleri doldur (tam bölünmemişse)
        while (allColors.Count < totalColorCount)
        {
            allColors.Add(baseColors[Random.Range(0, baseColors.Length)]);
        }

        Shuffle(allColors);

        int index = 0;
        foreach (var tile in allTiles)
        {
            List<ColorVector> colorsForTile = new List<ColorVector>();
            for (int i = 0; i < stackSizePerTile; i++)
            {
                colorsForTile.Add(allColors[index++]);
            }
            tile.SetColors(colorsForTile);
        }
    }

    private ColorVector[] GenerateBaseColors(int count)
    {
        // İstersen farklı sayıda ana renk desteklemek için bu metodu genişletebilirsin
        // Şimdilik 6'ya kadar destekliyoruz.
        ColorVector[] colors6 = new ColorVector[]
        {
            new ColorVector(1, 0, 0), // Kırmızı
            new ColorVector(0, 1, 0), // Yeşil
            new ColorVector(0, 0, 1), // Mavi
            new ColorVector(1, 1, 0), // Sarı
            new ColorVector(0, 1, 1), // Camgöbeği
            new ColorVector(1, 0, 1)  // Magenta
        };

        if (count <= 0) return new ColorVector[0];
        if (count >= colors6.Length) return colors6;

        ColorVector[] result = new ColorVector[count];
        System.Array.Copy(colors6, result, count);
        return result;
    }

    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
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
        foreach (var tile in allTiles)
        {
            for (int i = 0; i < stackSizePerTile && index < allColors.Count; i++)
            {
                tile.PushColor(allColors[index]);
                index++;
            }
            tile.UpdateVisual();
        }
    }
}
