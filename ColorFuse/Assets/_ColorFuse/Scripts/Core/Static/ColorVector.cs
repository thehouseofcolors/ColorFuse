using UnityEngine;

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
