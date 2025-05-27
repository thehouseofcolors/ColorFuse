
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [Header("Particles")]
    public ParticleSystem colorCreatedEffect;
    public ParticleSystem colorCreationFailedEffect;
    public ParticleSystem levelStartEffect;
    public ParticleSystem levelCompleteEffect;

    [Header("Audio Clips")]
    public AudioSource audioSource;
    public AudioClip colorCreatedSound;
    public AudioClip colorCreationFailedSound;
    public AudioClip levelStartSound;
    public AudioClip levelCompleteSound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayColorCreatedEffect(Vector3 position)
    {
        if (colorCreatedEffect != null)
        {
            colorCreatedEffect.transform.position = position;
            colorCreatedEffect.Play();
        }
        PlaySound(colorCreatedSound);
    }

    public void PlayColorCreationFailedEffect(Vector3 position)
    {
        if (colorCreationFailedEffect != null)
        {
            colorCreationFailedEffect.transform.position = position;
            colorCreationFailedEffect.Play();
        }
        PlaySound(colorCreationFailedSound);
    }

    public void PlayLevelStartEffect()
    {
        if (levelStartEffect != null) levelStartEffect.Play();
        PlaySound(levelStartSound);
    }

    public void PlayLevelCompleteEffect()
    {
        if (levelCompleteEffect != null) levelCompleteEffect.Play();
        PlaySound(levelCompleteSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
