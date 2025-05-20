using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Color highlightColor = Color.white;

    private List<Tile> selectedTiles = new();

    public void SelectTile(Tile tile)
    {
        if (selectedTiles.Contains(tile))
        {
            // Zaten seçiliyse bırak
            tile.SetHighlight(false);
            selectedTiles.Remove(tile);
            return;
        }

        if (selectedTiles.Count >= 2)
        {
            Debug.Log("En fazla 2 taş seçilebilir.");
            return;
        }

        selectedTiles.Add(tile);
        tile.SetHighlight(true);

        if (selectedTiles.Count == 2)
        {
            TryCombineTiles();
        }
    }

    void TryCombineTiles()
    {
        var tileA = selectedTiles[0];
        var tileB = selectedTiles[1];

        if (CanCombine(tileA.tileColor, tileB.tileColor, out TileColor resultColor))
        {
            tileA.SetColor(resultColor);
            Destroy(tileB.gameObject);
            selectedTiles.Clear();
        }
        else
        {
            Debug.Log("Geçersiz kombinasyon!");
            // Buraya animasyon/efekt çağırabilirsin
            foreach (var tile in selectedTiles)
                tile.SetHighlight(false);

            selectedTiles.Clear();
        }
    }

    bool CanCombine(TileColor a, TileColor b, out TileColor result)
    {
        result = TileColor.Red; // Varsayılan
        // Basit örnek kurallar:
        if ((a == TileColor.Red && b == TileColor.Blue) || (a == TileColor.Blue && b == TileColor.Red))
        {
            result = TileColor.Purple;
            return true;
        }
        if ((a == TileColor.Red && b == TileColor.Yellow) || (a == TileColor.Yellow && b == TileColor.Red))
        {
            result = TileColor.Orange;
            return true;
        }
        if ((a == TileColor.Blue && b == TileColor.Yellow) || (a == TileColor.Yellow && b == TileColor.Blue))
        {
            result = TileColor.Green;
            return true;
        }

        return false;
    }
}
