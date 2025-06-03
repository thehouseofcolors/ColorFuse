using System.Collections.Generic;
using UnityEngine;

public class UndoManager : Singleton<UndoManager>, IGameSystem
{
    private Stack<IUndoAction> undoStack = new Stack<IUndoAction>();

    [SerializeField] private int maxUndoCount = 3;
    [SerializeField] private int maxUndoUses = 3;

    private int totalUndoUses;

    public int UndoCount => undoStack.Count;
    public int RemainingUndoUses => maxUndoUses - totalUndoUses;

    public void Initialize()
    {
        totalUndoUses = 0;
        undoStack.Clear();
        Debug.Log("[UndoManager] Initialized.");
        EventBus.Subscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }

    public void Shutdown()
    {
        undoStack.Clear();
        totalUndoUses = 0;
        Debug.Log("[UndoManager] Shutdown completed and stack cleared.");
        EventBus.Unsubscribe<LevelEvents.LevelStartedEvent>(OnLevelStarted);
    }
    private void OnLevelStarted(LevelEvents.LevelStartedEvent e)
    {
        ResetUndoStack();
    }
    public void RecordAction(IUndoAction action)
    {
        if (undoStack.Count >= maxUndoCount)
        {
            // Eski en alttaki (en eski) undo'yu çıkarıyoruz
            var tempList = new List<IUndoAction>(undoStack);
            tempList.RemoveAt(0); // En eski eleman (son eleman stack'te) 0 indexidir

            undoStack.Clear();
            for (int i = tempList.Count - 1; i >= 0; i--)
                undoStack.Push(tempList[i]);
        }

        undoStack.Push(action);
        Debug.Log($"[UndoManager] Undo kaydedildi. Undo sayısı: {undoStack.Count}");
    }

    public bool CanUndo()
    {
        bool can = undoStack.Count > 0 && totalUndoUses < maxUndoUses;
        Debug.Log($"[UndoManager] CanUndo kontrolü: {can}");
        return can;
    }

    public bool Undo()
    {
        if (!CanUndo())
        {
            Debug.LogWarning("[UndoManager] Undo yapılamıyor: hakkınız kalmamış veya undo yok.");
            return false;
        }

        var action = undoStack.Pop();
        action.Undo();
        totalUndoUses++;
        Debug.Log($"[UndoManager] Undo yapıldı. Kalan undo hakkı: {RemainingUndoUses}");
        return true;
    }

    public void ResetUndoStack()
    {
        undoStack.Clear();
        totalUndoUses = 0;
        Debug.Log("[UndoManager] Undo stack sıfırlandı.");
    }
}
