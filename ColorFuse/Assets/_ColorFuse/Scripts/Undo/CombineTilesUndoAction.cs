

public class CombineTilesUndoAction : IUndoAction
{
    private Tile tileA;
    private Tile tileB;
    private ColorVector originalColorA;
    private ColorVector originalColorB;
    private bool hadResultColor; // Son tile'a ara renk eklenmiş miydi?

    public CombineTilesUndoAction(Tile tileA, Tile tileB, ColorVector originalColorA, ColorVector originalColorB, bool hadResultColor)
    {
        this.tileA = tileA;
        this.tileB = tileB;
        this.originalColorA = originalColorA;
        this.originalColorB = originalColorB;
        this.hadResultColor = hadResultColor;
    }

    public void Undo()
    {
        // Eğer ara renk eklenmişse, onu sil
        if (hadResultColor)
        {
            tileB.PopTopColor();
        }

        // Eski renkleri geri koy
        tileA.PushColor(originalColorA);
        tileB.PushColor(originalColorB);
    }
}

