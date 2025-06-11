
using UnityEngine;

public static class ColorFusion
{
    public static bool CanMerge(ColorVector a, ColorVector b)
    {
        var merged = a + b;
        return merged.IsValidColor;
    }

    public static ColorVector Merge(ColorVector a, ColorVector b)
    {
        return new ColorVector(
            Mathf.Clamp(a.R + b.R, 0, 1),
            Mathf.Clamp(a.G + b.G, 0, 1),
            Mathf.Clamp(a.B + b.B, 0, 1)
        );
    }
}