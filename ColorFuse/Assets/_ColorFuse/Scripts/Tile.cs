using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer;

    public TileColor tileColor { get; private set; }

    public void SetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetColor(TileColor color)
    {
        tileColor = color;
        spriteRenderer.color = GetColorFromEnum(color);
    }

    private Color GetColorFromEnum(TileColor color)
    {
        return color switch
        {
            TileColor.Red => Color.red,
            TileColor.Blue => Color.blue,
            TileColor.Yellow => Color.yellow,
            TileColor.Green => Color.green,
            TileColor.Orange => new Color(1f, 0.5f, 0f),
            TileColor.Purple => new Color(0.5f, 0f, 1f),
            _ => Color.white
        };
    }
    [SerializeField] private GameObject highlightVisual; // Örneğin glow efekti

    private SelectionManager selectionManager;

    void Start()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
    }

    void OnMouseDown()
    {
        Debug.Log("Tile clicked: " + tileColor);
        selectionManager.SelectTile(this);
    }

    public void SetHighlight(bool on)
    {
        if (highlightVisual != null)
            highlightVisual.SetActive(on);
    }

}
