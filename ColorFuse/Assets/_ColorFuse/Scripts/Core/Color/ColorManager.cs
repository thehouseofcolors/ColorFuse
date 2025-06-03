
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorCombo
{
    public List<ColorVector> parts;

    public ColorCombo(params ColorVector[] vectors)
    {
        parts = new List<ColorVector>(vectors);
    }

    public ColorVector GetResult()
    {
        ColorVector result = new ColorVector(0, 0, 0);
        foreach (var v in parts)
            result += v;
        return result;
    }
}


public static class ColorManager
{
    private static readonly ColorVector[] tileColors = new ColorVector[]
    {
        new ColorVector(1, 0, 0), // Kırmızı
        new ColorVector(0, 1, 0), // Yeşil
        new ColorVector(0, 0, 1), // Mavi
        new ColorVector(1, 1, 0), // Sarı
        new ColorVector(0, 1, 1), // Camgöbeği
        new ColorVector(1, 0, 1)  // Magenta
    };

    private static readonly List<ColorCombo> whiteCombos = new List<ColorCombo>()
    {
        new ColorCombo(new ColorVector(1,0,0), new ColorVector(0,1,0), new ColorVector(0,0,1)),
        new ColorCombo(new ColorVector(1,1,0), new ColorVector(0,0,1)),
        new ColorCombo(new ColorVector(0,1,1), new ColorVector(1,0,0)),
        new ColorCombo(new ColorVector(1,0,1), new ColorVector(0,1,0))
    };

    public static ColorVector[] GetAllTileColors => tileColors;

    public static List<ColorVector> GetRandomWhite()
    {
        ColorCombo combo = whiteCombos[Random.Range(0, whiteCombos.Count)];
        return combo.parts;
    }

    public static List<ColorCombo> GetAllWhiteCombos()
    {
        return whiteCombos;
    }

    public static ColorVector CombineColors(ColorVector a, ColorVector b)
    {
        int r = Mathf.Clamp(a.R + b.R, 0, 1);
        int g = Mathf.Clamp(a.G + b.G, 0, 1);
        int b_ = Mathf.Clamp(a.B + b.B, 0, 1);

        ColorVector result = new ColorVector(r, g, b_);

        if (r == 0 && g == 0 && b_ == 0)
        {
            Debug.LogWarning("Geçersiz renk karışımı: siyah oluştu.");
        }

        return result;
    }
}
