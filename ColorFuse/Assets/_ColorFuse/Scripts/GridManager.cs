using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    [SerializeField] private float spacing = 1.1f;
    [SerializeField] private GameObject tilePrefab;

    private GameObject[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new GameObject[width, height];

        // Gridin merkezini hesapla
        float gridWidth = (width - 1) * spacing;
        float gridHeight = (height - 1) * spacing;
        Vector2 offset = new Vector2(gridWidth, gridHeight) / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x * spacing, y * spacing);
                position -= offset; // Ortalamak için offset uygula

                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                grid[x, y] = tile;

                var tileComponent = tile.GetComponent<Tile>();
                if (tileComponent != null)
                {
                    tileComponent.SetCoordinates(x, y);
                    tileComponent.SetColor(GetRandomColor());
                }
            }
        }
    }

    TileColor GetRandomColor()
    {
        int colorCount = System.Enum.GetValues(typeof(TileColor)).Length;
        return (TileColor)Random.Range(0, colorCount);
    }

}


