// using UnityEngine;
// using System.Collections;

// public class AnimationManager : MonoBehaviour, IGameSystem
// {
//     [SerializeField] private GameObject successEffectPrefab;
//     [SerializeField] private GameObject failEffectPrefab;

//     public void Initialize()
//     {
//         EventBus.Subscribe<TileEvents.WhiteTileCollectedEvent>(OnSuccess);
//         EventBus.Subscribe<TileEvents.TileRelasedEvent>(OnFailed);
//     }

//     public void Shutdown()
//     {
//         EventBus.Unsubscribe<TileEvents.WhiteTileCollectedEvent>(OnSuccess);
//         EventBus.Unsubscribe<TileEvents.TileRelasedEvent>(OnFailed);
//     }

//     private void OnSuccess(TileEvents.WhiteTileCollectedEvent e)
//     {
//         SpawnEffect(successEffectPrefab, transform.position);

//         // Bounce animasyonu
//         StartCoroutine(PlaySuccessAnimation(transform));
//     }

//     private void OnFailed(TileEvents.TileRelasedEvent e)
//     {
//         SpawnEffect(failEffectPrefab, transform.position);

//         // Tile salla
//         StartCoroutine(Shake(transform));

//         // Kamera salla
//         CameraShake.Instance?.Shake(0.2f, 0.2f);
//     }


//     private void SpawnEffect(GameObject prefab, Vector3 position)
//     {
//         if (prefab == null) return;

//         GameObject fx = Instantiate(prefab, position, Quaternion.identity);
//         Destroy(fx, 1.5f);
//     }

//     private IEnumerator PlaySuccessAnimation(Transform target)
//     {
//         Vector3 original = target.localScale;
//         Vector3 big = original * 1.25f;
//         float time = 0.15f;

//         yield return ScaleTween(target, original, big, time);
//         yield return ScaleTween(target, big, original, time);
//     }

//     private IEnumerator ScaleTween(Transform target, Vector3 from, Vector3 to, float duration)
//     {
//         float t = 0f;
//         while (t < duration)
//         {
//             target.localScale = Vector3.Lerp(from, to, t / duration);
//             t += Time.deltaTime;
//             yield return null;
//         }
//         target.localScale = to;
//     }

//     private IEnumerator Shake(Transform target, float duration = 0.3f, float magnitude = 0.05f)
//     {
//         Vector3 original = target.localPosition;
//         float elapsed = 0f;

//         while (elapsed < duration)
//         {
//             float x = Random.Range(-1f, 1f) * magnitude;
//             float y = Random.Range(-1f, 1f) * magnitude;
//             target.localPosition = original + new Vector3(x, y, 0);

//             elapsed += Time.deltaTime;
//             yield return null;
//         }

//         target.localPosition = original;
//     }
// }
