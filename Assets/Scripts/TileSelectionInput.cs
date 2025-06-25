using UnityEngine;
using UnityEngine.InputSystem;
using GameEvents;

public class TileSelectionInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private GameInput inputActions;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        inputActions = new GameInput();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Newactionmap.TileClick.performed += OnTileClickPerformed;
    }

    private void OnDisable()
    {
        inputActions.Newactionmap.TileClick.performed -= OnTileClickPerformed;
        inputActions.Disable();
    }

    private void OnTileClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPosition);
        if (hit != null)
        {
            Tile tile = hit.GetComponent<Tile>();
            if (tile != null && tile.CanSelectable)
            {
                tile.HandleSelection().Forget();
            }
        }
    }
}
