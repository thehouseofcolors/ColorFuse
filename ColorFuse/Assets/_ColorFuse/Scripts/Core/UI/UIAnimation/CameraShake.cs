using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    private Vector3 originalPos;
    private float shakeTimeRemaining = 0f;

    private void Awake()
    {
        Instance = this;
        originalPos = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;

            if (shakeTimeRemaining <= 0f)
                cameraTransform.localPosition = originalPos;
        }
    }

    public void ShakeCamera()
    {
        shakeTimeRemaining = shakeDuration;
    }
}
