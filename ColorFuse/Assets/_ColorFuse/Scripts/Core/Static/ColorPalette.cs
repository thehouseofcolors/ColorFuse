
using System;
using System.Collections;

public static class ColorPalette
{
    private static readonly ColorVector[] palette = new ColorVector[]
    {
        new ColorVector(1, 0, 0),
        new ColorVector(0, 1, 0),
        new ColorVector(0, 0, 1),
        new ColorVector(1, 1, 0),
        new ColorVector(0, 1, 1),
        new ColorVector(1, 0, 1)
    };

    public static ColorVector GetColorVector(int index) => palette[index];
    public static int GetColorIndex(ColorVector vector) => Array.IndexOf(palette, vector);

    

}

