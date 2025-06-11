
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SeedGenerator
{
    public static List<ColorVector> GenerateAllTileColors(int whiteComboCount)
    {
        var colors = new List<ColorVector>();
        for (int i = 0; i < whiteComboCount; i++)
        {
            colors.AddRange(WhiteColorCombos.GetRandomCombo());
        }
        colors.Shuffle();
        return colors;
    }

    public static string GenerateSeed(int whiteComboCount)
    {
        var colors = GenerateAllTileColors(whiteComboCount);

        return SeedEncoder.EncodeColorsToSeed(colors);
    }
}
