using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class TileUtility
{
    
    public static bool HasAnyValidCombination(List<Tile> tiles)
    {
        var activeTiles = tiles.Where(t => t.HasColors).ToList();
        for (int i = 0; i < activeTiles.Count; i++)
        {
            var colorA = activeTiles[i].PeekColor();
            for (int j = i + 1; j < activeTiles.Count; j++)
            {
                var colorB = activeTiles[j].PeekColor();
                var result = ColorManager.CombineColors(colorA, colorB);
                if (result.IsValidColor) return true;
            }
        }
        return false;
    }

}