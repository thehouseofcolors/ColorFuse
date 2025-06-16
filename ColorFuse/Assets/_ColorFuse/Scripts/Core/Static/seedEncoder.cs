
using System.Collections.Generic;

public static class SeedEncoder
{
    public static string EncodeColorsToSeed(List<ColorVector> colorVectors)
    {
        var builder = new System.Text.StringBuilder();
        foreach (var c in colorVectors)
        {
            builder.Append(ColorPalette.GetColorIndex(c));
        }
        return builder.ToString();
    }


    public static Stack<ColorVector> DecodeSeedToColors(string seed)
    {
        var colorVectors = new Stack<ColorVector>();
        foreach (char ch in seed)
        {
            int index = int.Parse(ch.ToString());
            ColorVector vector = ColorPalette.GetColorVector(index);
            colorVectors.Push(vector);
        }
        return colorVectors;
    }

}


