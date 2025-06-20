using System.Collections;
using UnityEngine;
using GameEvents;
using System.Threading.Tasks;

public class TileSelectionHandler : MonoBehaviour, IGameSystem
{
    private Tile firstTile;
    private Tile secondTile;
    private bool isProcessing = false;
    
    private SelectionState currentState = SelectionState.None;
    
    private enum SelectionState
    {
        None,
        FirstSelected,
        SecondSelected,
        Processing
    }

    public void Initialize()
    {
        EventBus.Subscribe<TileSelectionEvent>(OnSelectTile);
    }

    public void Shutdown()
    {
        ClearSelection();
    }

    public async Task OnSelectTile(TileSelectionEvent e)
    {
        if (currentState == SelectionState.Processing)
        {
            Debug.LogWarning("[SelectionManager] Selection blocked - processing in progress");
            return;
        }

        if (e.Tile == null)
        {
            Debug.LogWarning("[SelectionManager] Received null tile selection");
            return;
        }

        // Prevent selecting the same tile twice
        if (e.Tile == firstTile || e.Tile == secondTile)
        {
            Debug.Log($"[SelectionManager] Tile {e.Tile.name} already selected - clearing selection");
            ClearSelection();
            return;
        }

        switch (currentState)
        {
            case SelectionState.None:
                firstTile = e.Tile;
                firstTile.SetHighlight(true);
                currentState = SelectionState.FirstSelected;
                break;
                
            case SelectionState.FirstSelected:
                secondTile = e.Tile;
                secondTile.SetHighlight(true);
                currentState = SelectionState.SecondSelected;
                await ProcessSelection();
                break;
        }
    }

    private async Task ProcessSelection()
    {
        currentState = SelectionState.Processing;
        
        try
        {
            await EventBus.PublishAsync(new TileFuseEvent(firstTile, secondTile));
        }
        finally
        {
            ClearSelection();
        }
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

        currentState = SelectionState.None;
        Debug.Log("[SelectionManager] Selection cleared");
    }
}