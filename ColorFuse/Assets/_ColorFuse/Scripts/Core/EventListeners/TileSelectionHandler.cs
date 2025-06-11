using System.Collections;
using UnityEngine;
using SelectionEvents;
using System.Threading.Tasks;
public class TileSelectionHandler : MonoBehaviour, IGameSystem
{
    private Tile firstTile;
    private Tile secondTile;
    private bool isFirstSelected = false;
    private bool isSecondSelected = false;
    private bool isProcessing = false;


    public void Initialize()
    {
        EventBus.Subscribe<TileSelectedEvent>(OnSelectTile);
        
        Debug.Log("[SelectionManager] Initialized and subscribed to TileSelectedEvent.");
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<SelectionEvents.TileSelectedEvent>(OnSelectTile);
        Debug.Log("[SelectionManager] Shutdown and unsubscribed.");
    }
    

    public async Task OnSelectTile(SelectionEvents.TileSelectedEvent e)
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
            ClearSelection();//geri bırakma 
            return;
        }

        if (!isFirstSelected)
        {
            firstTile = e.SelectedTile;
            isFirstSelected = true;
            firstTile.SetHighlight(true);//burada seçilme animasyonu gösteriyor
        }
        else if (!isSecondSelected)
        {
            secondTile = e.SelectedTile;
            isSecondSelected = true;
            secondTile.SetHighlight(true);
            TryCombineTiles(firstTile, secondTile);
        }
    }
    public void OnDeselectTile(SelectionEvents.ClearTileSelectionEvent e)
    {
        ClearSelection();
        isProcessing = false;
    }


    private void TryCombineTiles(Tile a, Tile b)
    {
        TryCombineCoroutine(a, b);
    }
    private async Task TryCombineCoroutine(Tile a, Tile b)
    {
        isProcessing = true;


        await EventBus.PublishAsync(new FusionEvents.ColorMergeRequestedEvent(a, b));


    }

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
