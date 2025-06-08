
using TMPro;
using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour, IGameSystem
{
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float displayDuration = 1.2f;
    private Coroutine currentRoutine;

    public void Initialize()
    {
        EventBus.Subscribe<TileEvents.TileCombinedEvent>(OnCombined);
        EventBus.Subscribe<TileEvents.TileRelasedEvent>(OnRelased);
        EventBus.Subscribe<TileEvents.WhiteTileCollectedEvent>(OnWhiteFormed);
    }

    public void Shutdown()
    {
        EventBus.Unsubscribe<TileEvents.TileCombinedEvent>(OnCombined);
        EventBus.Unsubscribe<TileEvents.TileRelasedEvent>(OnRelased);
        EventBus.Unsubscribe<TileEvents.WhiteTileCollectedEvent>(OnWhiteFormed);
    }

    private void ShowMessage(string message, Color color)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        notificationText.text = message;
        notificationText.color = new Color(color.r, color.g, color.b, 1f);
        notificationText.gameObject.SetActive(true);

        currentRoutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float time = 0f;
        Color startColor = notificationText.color;

        while (time < displayDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / displayDuration);
            notificationText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        notificationText.gameObject.SetActive(false);
    }

    private void OnCombined(TileEvents.TileCombinedEvent e)
    {
        ShowMessage("✔️ Birleşti!", Color.green);
    }

    private void OnRelased(TileEvents.TileRelasedEvent e)
    {
        ShowMessage("🚫 Uyuşmaz!", Color.red);
        CameraShaker.Instance.ShakeCamera(); // Aşağıda geliyor
    }

    private void OnWhiteFormed(TileEvents.WhiteTileCollectedEvent e)
    {
        ShowMessage("✨ Beyaz oluştu!", Color.white);
    }
}
