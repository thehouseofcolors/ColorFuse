using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    private Tile firstTile;
    private Tile secondTile;
    private bool isFirstSelected = false;
    private bool isSecondSelected = false;

    public void SelectTile(Tile tile)
    {
        // Aynı tile ikinci kez seçilmesin
        if (tile == firstTile || tile == secondTile)
        {
            Debug.LogWarning("Bu tile zaten seçili.");
            return;
        }

        // Boş slot bul ve ekle
        if (!isFirstSelected)
        {
            firstTile = tile;
            isFirstSelected = true;
            tile.SetHighlight(true);
            Debug.Log("İlk tile seçildi.");
        }
        else if (!isSecondSelected)
        {
            secondTile = tile;
            isSecondSelected = true;
            tile.SetHighlight(true);
            Debug.Log("İkinci tile seçildi, birleştirme deneniyor.");
            TryCombineTiles();
        }
    }

    void TryCombineTiles()
    {
        var colorA = firstTile.PeekColor();
        var colorB = secondTile.PeekColor();


        var resultColor = colorA + colorB;
        bool willAddResult = resultColor.IsValidColor;

        UndoManager.Instance.RecordAction(new CombineTilesUndoAction(firstTile, secondTile, colorA, colorB, willAddResult));

        if (resultColor.IsWhite)
        {
            Debug.Log("Beyaz oluştu! İki taş silindi.");
            firstTile.PopTopColor();
            secondTile.PopTopColor();

            EventBus.Publish(new TileStateChangedEvent());  
            EventBus.Publish(new WhiteColorFormedEvent(secondTile));
        }
        else if (resultColor.IsValidColor)
        {
            Debug.Log("Ara renk oluştu! İki taş silindi, ara renk son tıklanan taşta gösteriliyor.");
            firstTile.PopTopColor();
            secondTile.PopTopColor();
            secondTile.PushColor(resultColor);

            EventBus.Publish(new TileStateChangedEvent());
            EventBus.Publish(new ColorCombinedEvent(firstTile, secondTile, resultColor));
        }
        else
        {
            
            EventBus.Publish(new TileStateChangedEvent());
            Debug.Log("Geçersiz kombinasyon, hiçbir şey yapılmadı.");
            EventBus.Publish(new InvalidCombineEvent(firstTile, secondTile));
        }

        ClearSelection();
    }

    void ClearSelection()
    {
        if (isFirstSelected)
        {
            firstTile.SetHighlight(false);
            firstTile = null;
            isFirstSelected = false;
        }

        if (isSecondSelected)
        {
            secondTile.SetHighlight(false);
            secondTile = null;
            isSecondSelected = false;
        }

        Debug.Log("Seçimler temizlendi.");
    }
}
