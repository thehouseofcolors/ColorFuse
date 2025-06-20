using UnityEngine;
using System.Collections;

public class CameraShaker : Singleton<CameraShaker>
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float defaultShakeDuration = 0.2f;
    [SerializeField] private float defaultShakeMagnitude = 0.1f;

    private Vector3 originalPos;

    public void Awake()
    {
        originalPos = cameraTransform.localPosition;
    }

    

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            cameraTransform.localPosition = originalPos + Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }

    // Eğer elle tetiklemek istersen:
    public void ShakeCamera()
    {
        StartCoroutine(Shake(defaultShakeDuration, defaultShakeMagnitude));
    }
}
