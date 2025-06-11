
using System.Collections.Generic;

[System.Serializable]
public struct ColorCombo
{
    public List<ColorVector> parts;

    public ColorCombo(params ColorVector[] vectors)
    {
        parts = new List<ColorVector>(vectors);
    }
}

public static class WhiteColorCombos
{
    private static readonly List<ColorCombo> combos = new List<ColorCombo>()
    {
        new ColorCombo(new ColorVector(1,0,0), new ColorVector(0,1,0), new ColorVector(0,0,1)),
        new ColorCombo(new ColorVector(1,1,0), new ColorVector(0,0,1)),
        new ColorCombo(new ColorVector(0,1,1), new ColorVector(1,0,0)),
        new ColorCombo(new ColorVector(1,0,1), new ColorVector(0,1,0))
    };

    public static List<ColorVector> GetRandomCombo()
    {
        return combos[UnityEngine.Random.Range(0, combos.Count)].parts;
    }

    public static List<ColorVector> GetRandomCombo(System.Random rng)
    {
        return combos[rng.Next(combos.Count)].parts;
    }
}
