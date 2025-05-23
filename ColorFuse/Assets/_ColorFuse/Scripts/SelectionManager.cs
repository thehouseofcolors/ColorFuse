using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    private Tile firstTile;
    private Tile secondTile;
    private bool isFirstSelected = false;
    private bool isSecondSelected = false;

    public void SelectTile(Tile tile)
    {
        // Aynı tile tekrar seçildiyse kaldır
        if (isFirstSelected && firstTile == tile)
        {
            tile.SetHighlight(false);
            firstTile = null;
            isFirstSelected = false;
            Debug.Log("İlk seçim iptal edildi.");
            return;
        }

        if (isSecondSelected && secondTile == tile)
        {
            tile.SetHighlight(false);
            secondTile = null;
            isSecondSelected = false;
            Debug.Log("İkinci seçim iptal edildi.");
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

        if (colorA.Equals(new ColorVector(0, 0, 0)) || colorB.Equals(new ColorVector(0, 0, 0)))
        {
            Debug.Log("Seçilen taşlardan en az birinde renk yok.");
            ClearSelection();
            return;
        }

        var resultColor = colorA + colorB;

            
        // Undo kaydını birleştirmeden önce yap
        bool willAddResult = resultColor.IsValidColor && !resultColor.IsWhite;
        

        if (resultColor.IsWhite)
        {
            UndoManager.Instance.RecordAction(new CombineTilesUndoAction(firstTile, secondTile, colorA, colorB, willAddResult));

            Debug.Log("Beyaz oluştu! İki taş silindi.");
            firstTile.PopTopColor();
            secondTile.PopTopColor();
            ClearSelection();
            return;
        }

        if ((colorA + colorB).IsValidColor)
        {
            UndoManager.Instance.RecordAction(new CombineTilesUndoAction(firstTile, secondTile, colorA, colorB, willAddResult));

            Debug.Log("Ara renk oluştu! İki taş silindi, ara renk son tıklanan taşta gösteriliyor.");
            firstTile.PopTopColor();
            secondTile.PopTopColor();
            secondTile.PushColor(resultColor); // Son tıklanan tile'da göster
            ClearSelection();
            return;
        }

        Debug.Log("Geçersiz kombinasyon, hiçbir şey yapılmadı.");
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

