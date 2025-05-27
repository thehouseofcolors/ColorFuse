
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

public class ColorManager : Singleton<ColorManager>
{
    private ColorVector[] tileColors = new ColorVector[]
    {
        new ColorVector(1, 0, 0), // Kırmızı
        new ColorVector(0, 1, 0), // Yeşil
        new ColorVector(0, 0, 1), // Mavi
        new ColorVector(1, 1, 0), // Sarı
        new ColorVector(0, 1, 1), // Camgöbeği
        new ColorVector(1, 0, 1)  // Magenta
    };

    public ColorVector[] GetAllTileColors => tileColors;

    

    private List<ColorCombo> whiteCombos = new List<ColorCombo>()
    {
        new ColorCombo(new ColorVector(1,0,0),new ColorVector(0,1,0), new ColorVector(0,0,1)),
        new ColorCombo(new ColorVector(1,1,0), new ColorVector(0,0,1)), // Sarı + Mavi
        new ColorCombo(new ColorVector(0,1,1), new ColorVector(1,0,0)), // Camgöbeği + Kırmızı
        new ColorCombo(new ColorVector(1,0,1), new ColorVector(0,1,0))  // Magenta + Yeşil
    };
    public List<ColorVector> GetRandomWhite()
    {
        ColorCombo colorCombo = whiteCombos[Random.Range(0, whiteCombos.Count)];
        return colorCombo.parts;
    }

    public List<ColorCombo> GetAllWhiteCombos()
    {
        return whiteCombos;
    }
}
