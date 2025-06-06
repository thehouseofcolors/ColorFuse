
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

    // public ColorVector GetResult()
    // {
    //     ColorVector result = new ColorVector(0, 0, 0);
    //     foreach (var v in parts)
    //         result += v;
    //     return result;
    // }
}

[System.Serializable]
public struct ColorVector
{
    public int R, G, B;

    public ColorVector(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
    }
    public override string ToString()
    {
        return $"ColorVector(R={R}, G={G}, B={B})";
    }


    public static ColorVector operator +(ColorVector a, ColorVector b)
    {
        return new ColorVector(a.R + b.R, a.G + b.G, a.B + b.B);
    }

    public Color ToUnityColor()
    {
        return new Color(R, G, B);
    }

    public bool IsWhite => R == 1 && G == 1 && B == 1;

    public bool IsValidColor =>
        (R == 0 || R == 1) && (G == 0 || G == 1) && (B == 0 || B == 1)
        && (R + G + B > 0); // Siyah değil



    public bool IsBaseColor => 
        (R + G + B == 1) && IsValidColor;

    public bool IsIntermediateColor => 
        (R + G + B == 2) && IsValidColor;

}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
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

    public static List<ColorCombo> GetAllWhiteCombos => whiteCombos;

    public static List<ColorVector> GetRandomWhite()
    {
        ColorCombo combo = whiteCombos[Random.Range(0, whiteCombos.Count)];
        return combo.parts;
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


    public static Stack<ColorVector> GenerateColors(int whiteComboCount)
    {
        var colors = new List<ColorVector>();

        for (int i = 0; i < whiteComboCount; i++)
        {
            var combo = ColorManager.GetRandomWhite();
            colors.AddRange(combo);
        }

        colors.Shuffle();
        Stack<ColorVector> colorStack = new Stack<ColorVector>(colors);
        return colorStack;
    }

}
