using System.Collections;
using UnityEngine;

public class ColorCombinationManager : MonoBehaviour, IGameSystem
{
    private Tile firstTile;
    private Tile secondTile;
    private bool isFirstSelected = false;
    private bool isSecondSelected = false;
    private bool isProcessing = false;


    public void Initialize()
    {
        EventBus.Subscribe<TileEvents.TileSelectedEvent>(OnSelectTile);
        Debug.Log("[SelectionManager] Initialized and subscribed to TileSelectedEvent.");
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<TileEvents.TileSelectedEvent>(OnSelectTile);
        Debug.Log("[SelectionManager] Shutdown and unsubscribed.");
    }
    

    public void OnSelectTile(TileEvents.TileSelectedEvent e)
    {
        if (isProcessing)
        {
            Debug.LogWarning("[SelectionManager] İşlem devam ederken yeni seçim engellendi.");
            return;
        }

        if (e.SelectedTile == null)
        {
            Debug.LogWarning("[SelectionManager] Received null TileSelectedEvent or SelectedTile.");
            return;
        }

        // Aynı tile tekrar seçilmesin
        if (e.SelectedTile == firstTile || e.SelectedTile == secondTile)
        {
            Debug.LogWarning($"[SelectionManager] Tile '{e.SelectedTile.name}' zaten seçili.");
            return;
        }

        if (!isFirstSelected)
        {
            firstTile = e.SelectedTile;
            isFirstSelected = true;
            firstTile.SetHighlight(true);
            Debug.Log($"[SelectionManager] İlk tile seçildi: {firstTile.name}");
        }
        else if (!isSecondSelected)
        {
            secondTile = e.SelectedTile;
            isSecondSelected = true;
            secondTile.SetHighlight(true);
            Debug.Log($"[SelectionManager] İkinci tile seçildi: {secondTile.name}, birleşme kontrolü yapılıyor.");
            TryCombineTiles(firstTile, secondTile);
        }
    }
    private void TryCombineTiles(Tile a, Tile b)
    {
        StartCoroutine(TryCombineCoroutine(a, b));
    }
    private IEnumerator TryCombineCoroutine(Tile a, Tile b)
    {
        isProcessing = true;

        yield return new WaitForSeconds(0.15f); // animasyon ya da efekt süresi gibi

        if (a == null || b == null)
        {
            Debug.LogError("[SelectionManager] TryCombineTiles: Tile'lar null!");
            ClearSelection();
            isProcessing = false;
            yield break;
        }

        var colorA = a.PeekColor();
        var colorB = b.PeekColor();

        if (colorA.Equals(new ColorVector(0, 0, 0)) || colorB.Equals(new ColorVector(0, 0, 0)))
        {
            Debug.LogWarning("[SelectionManager] Tile'larda renk yok. Birleşme iptal.");
            ClearSelection();
            isProcessing = false;
            yield break;
        }

        var combinedColor = ColorManager.CombineColors(colorA, colorB);

        EventBus.Publish(new ColorEvents.TryColorCombinedEvent(a, b, combinedColor));

        yield return new WaitForSeconds(0.25f); // efekt sonrası bekleme

        ClearSelection();
        isProcessing = false;
    }


    // private void TryCombineTiles(Tile a, Tile b)
    // {
    //     if (a == null || b == null)
    //     {
    //         Debug.LogError("[SelectionManager] TryCombineTiles: Tile'lar null!");
    //         ClearSelection();
    //         return;
    //     }

    //     var colorA = a.PeekColor();
    //     var colorB = b.PeekColor();

    //     if (colorA.Equals(new ColorVector(0, 0, 0)) || colorB.Equals(new ColorVector(0, 0, 0)))
    //     {
    //         Debug.LogWarning("[SelectionManager] Tile'larda renk yok. Birleşme iptal.");
    //         ClearSelection();
    //         return;
    //     }

    //     var combinedColor = ColorManager.CombineColors(colorA, colorB);

    //     EventBus.Publish(new ColorEvents.TryColorCombinedEvent(a, b, combinedColor));

    //     ClearSelection();
    // }

    private void ClearSelection()
    {
        if (firstTile != null)
        {
            firstTile.SetHighlight(false);
            firstTile = null;
        }

        if (secondTile != null)
        {
            secondTile.SetHighlight(false);
            secondTile = null;
        }

        isFirstSelected = false;
        isSecondSelected = false;

        Debug.Log("[SelectionManager] Seçimler temizlendi.");
    }

}
