using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

// Her tile objeye ekle
[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{

#if UNITY_EDITOR
    [SerializeField, Tooltip("Only for debug view")]
    private List<ColorVector> debugColorList = new List<ColorVector>();
#endif

    public void UpdateDebugList()
    {
#if UNITY_EDITOR
        debugColorList = ColorStack.ToList();
#endif
    }

    Stack<ColorVector> ColorStack = new Stack<ColorVector>();

    public bool IsEmpty => ColorStack.Count <= 0;
    public bool IsSelectable;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public int X { get; private set; }
    public int Y { get; private set; }

    private void Awake()
    {
        // SpriteRenderer atandı mı? Eğer SerializeField boşsa, otomatik bağla
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("[Tile] SpriteRenderer missing on GameObject: " + gameObject.name);
            }

        }
    }


    private void OnMouseDown()
    {
        if (!enabled || !IsSelectable) return;

        EventBus.Publish(new TileEvents.TileSelectedEvent(this));
    }

    public void SetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public ColorVector PopTopColor()
    {
        if (ColorStack.Count == 0)
        {
            Debug.LogWarning("[Tile] Attempted to pop color from empty stack on tile: " + name);
            return new ColorVector(0, 0, 0); // ya da ColorVector.Invalid
        }

        var color = ColorStack.Pop();
        UpdateVisual();
        return color;
    }

    public void PushColor(ColorVector color)
    {
        if (!color.IsValidColor)  // İsteğe bağlı bir kontrol
        {
            Debug.LogWarning("[Tile] Trying to push invalid color on tile: " + name);
        }
        Debug.Log("color add to the tile");
        ColorStack.Push(color);
        UpdateVisual();
        UpdateDebugList();
    }

    public ColorVector PeekColor()
    {
        return ColorStack.Count > 0 ? ColorStack.Peek() : new ColorVector(0, 0, 0); // İstersen buraya da IsValid olmayan bir default üret
    }

    public void UpdateVisual()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("[Tile] SpriteRenderer is null when updating visuals.");
            return;
        }

        if (!IsEmpty)
        {
            spriteRenderer.color = ColorStack.Peek().ToUnityColor();
        }
        else
        {
            spriteRenderer.color = new Color(0, 0, 0, 0); // Şeffaf
        }

        SetSelectableStatus(IsEmpty);
    }

    public void SetSelectableStatus(bool isSelectable)
    {
        IsSelectable = !isSelectable;
        if (IsEmpty)
        {
            EventBus.Publish(new TileEvents.TileEmptiedEvent(this));
        }
    }

    public void SetHighlight(bool on)
    {
        transform.localScale = on ? Vector3.one * 1.2f : Vector3.one;
    }

    public IEnumerator PlayDestroyAnimation(float delay = 0.25f)
    {
        // Örnek: shrink veya fade animasyonu
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float duration = delay;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        Destroy(gameObject);
    }
    public IEnumerator PlayClearColorAnimation(float delay = 0.25f)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 0.5f;
        float duration = delay;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(0, 0, 0, 0), time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = startScale;
        spriteRenderer.color = new Color(0, 0, 0, 0);
        ClearAllColors();
    }
    public void ClearAllColors()
    {
        ColorStack.Clear();
        UpdateDebugList();
        UpdateVisual(); // Şeffaf hâle getirecek
    }



}
