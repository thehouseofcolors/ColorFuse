using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    [SerializeField] private int stackSizePerTile = 3;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private float spacing = 1.1f;

    private List<Tile> allTiles = new List<Tile>();

    void Start()
    {
        GenerateGrid();
        GenerateAndAssignColors();
    }

    void GenerateGrid()
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

    void GenerateAndAssignColors()
    {
        int totalTileCount = width * height;
        int totalColorCount = totalTileCount * stackSizePerTile;

        List<ColorVector> allColors = new List<ColorVector>();

        // Tanımlı 6 ana rengi oluştur
        ColorVector[] baseColors = new ColorVector[]
        {
            new ColorVector(1, 0, 0), // Kırmızı
            new ColorVector(0, 1, 0), // Yeşil
            new ColorVector(0, 0, 1), // Mavi
            new ColorVector(1, 1, 0), // Sarı
            new ColorVector(0, 1, 1), // Camgöbeği
            new ColorVector(1, 0, 1)  // Magenta
        };

        int countPerColor = totalColorCount / baseColors.Length;

        for (int i = 0; i < baseColors.Length; i++)
        {
            for (int j = 0; j < countPerColor; j++)
            {
                allColors.Add(baseColors[i]);
            }
        }

        // Eksik varsa doldur (tam bölünmemişse)
        while (allColors.Count < totalColorCount)
        {
            allColors.Add(baseColors[Random.Range(0, baseColors.Length)]);
        }

        // Renkleri karıştır
        Shuffle(allColors);


        // Renkleri tile'lara sırayla dağıt
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


}


