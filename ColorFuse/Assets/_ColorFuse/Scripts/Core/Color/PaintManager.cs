
using UnityEngine;
using System.Collections.Generic;

public static class PaintManager
{
    public static void PaintTiles(List<Tile> tiles, Stack<ColorVector> colors)
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

