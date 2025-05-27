using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

public class Tile : MonoBehaviour
{
    public Stack<ColorVector> ColorStack { get; private set; } = new Stack<ColorVector>();

    [SerializeField] private SpriteRenderer spriteRenderer;
   

    public int X { get; private set; }
    public int Y { get; private set; }
    

    void OnMouseDown()
    {
        SelectionManager.Instance.SelectTile(this);

    }

    // public void SetColors(List<ColorVector> colors)
    // {
    //     ColorStack = new Stack<ColorVector>(colors);
    //     UpdateVisual();
    // }

    public void SetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public ColorVector PopTopColor()
    {
        var color = ColorStack.Pop();
        

        UpdateVisual();
        return color;
    }

    public void PushColor(ColorVector color)
    {
        ColorStack.Push(color);
        UpdateVisual();
    }

    public ColorVector PeekColor()
    {
        return ColorStack.Count > 0 ? ColorStack.Peek() : new ColorVector(0, 0, 0);
    }
    public void UpdateVisual()
    {
        if (ColorStack.Count > 0)
        {
            spriteRenderer.color = ColorStack.Peek().ToUnityColor();
        }
        else
        {
            // No colors left, destroy this tile object
            GridManager.Instance.CleanupDestroyedTiles();
            Destroy(gameObject);
        }
    }


    public void SetHighlight(bool on)
    {
        transform.localScale = on ? Vector3.one * 1.2f : Vector3.one;
    }

    public bool HasColors()
    {
        return ColorStack.Count > 0;
    }

}
