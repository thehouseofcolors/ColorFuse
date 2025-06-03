
using System.Collections.Generic;

// public class FakeTile : Tile
// {
//     private Stack<ColorVector> colors = new Stack<ColorVector>(); 
//     public string name;

//     public FakeTile(string name)
//     {
//         this.name = name;
//     }

//     public void PushColor(ColorVector color)
//     {
//         colors.Push(color);
//     }

//     public ColorVector PopTopColor()
//     {
//         if (colors.Count == 0) return new ColorVector(0,0,0);
//         return colors.Pop();
//     }

//     public ColorVector PeekColor()
//     {
//         if (colors.Count == 0) return new ColorVector(0,0,0);
//         return colors.Peek();
//     }

//     public int ColorCount => colors.Count;

//     public void SetHighlight(bool active) { /* boş bırakabiliriz */ }
//     public void UpdateVisual() { /* boş bırakabiliriz */ }
// }

public class TileModel
{
    private Stack<ColorVector> _colors = new Stack<ColorVector>();

    public void PushColor(ColorVector color) => _colors.Push(color);
    public ColorVector PopTopColor() => _colors.Count > 0 ? _colors.Pop() : new ColorVector(0, 0, 0);
    public ColorVector PeekColor() => _colors.Count > 0 ? _colors.Peek() : new ColorVector(0, 0, 0);
    public int ColorCount => _colors.Count;
}
