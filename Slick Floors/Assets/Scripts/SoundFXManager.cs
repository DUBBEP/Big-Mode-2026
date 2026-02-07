using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSourcePrefab;

    private float moppingTimer = 0f;
    public float moppingInterval = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        moppingTimer += Time.deltaTime;
    }

    public void playSoundFXClip(AudioClip clip, Transform sourceTransform, float volume = 1f, float playPortion = -1f)
    {
        // Debug.Log($"SoundFXManager: Playing sound effect '{clip.name}' at position {sourceTransform.position} with volume {volume}.");
        AudioSource audioSource = Instantiate(audioSourcePrefab, sourceTransform.position, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        if (playPortion > 0f)
            clipLength = clip.length * playPortion;
        Destroy(audioSource.gameObject, clipLength);

    }

    public void PlayMopSounds(AudioClip clip, Transform sourceTransform, float volume = 1f)
    {

        if (moppingTimer >= moppingInterval)
        {
            playSoundFXClip(clip, sourceTransform, volume);
            moppingTimer = 0f;
        }
    }

}