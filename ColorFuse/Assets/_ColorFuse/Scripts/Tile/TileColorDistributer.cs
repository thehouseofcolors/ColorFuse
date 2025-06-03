using System.Collections.Generic;
using UnityEngine;

public static class TileColorDistributor
{
    public static List<ColorVector> GenerateColors(int whiteComboCount)
    {
        var colors = new List<ColorVector>();

        for (int i = 0; i < whiteComboCount; i++)
        {
            var combo = ColorManager.GetRandomWhite();
            colors.AddRange(combo);
        }

        colors.Shuffle();
        return colors;
    }

    public static void DistributeColors(List<Tile> tiles, List<ColorVector> colors)
    {
        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogWarning($"TileColorDistributor: {tiles.Count}");
            return;
        }

        if (colors == null || colors.Count == 0)
        {
            Debug.LogWarning("TileColorDistributor: Color list is empty or null!");
            return;
        }

        int index = 0;
        int stackSize = colors.Count / tiles.Count;

        for (int i = 0; i < tiles.Count; i++)
        {
            var tile = tiles[i];

            for (int j = 0; j < stackSize && index < colors.Count; j++)
            {
                tile.PushColor(colors[index++]);
            }
        }

        while (index < colors.Count)
        {
            int randomTileIndex = Random.Range(0, tiles.Count);
            tiles[randomTileIndex].PushColor(colors[index++]);
        }

        foreach (var tile in tiles)
        {
            tile.UpdateVisual();
        }
    }
}
