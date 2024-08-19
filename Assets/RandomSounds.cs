using UnityEngine;

public class RandomAudioPlayerWithPitch : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip[] audioClips;  // Array to hold your audio clips
    public float minDelay = 1.0f;   // Minimum delay between clips
    public float maxDelay = 5.0f;   // Maximum delay between clips
    public bool playOnStart = true; // Should the sounds start playing automatically?

    [Header("Pitch Settings")]
    public float minPitch = 0.8f;  // Minimum pitch value
    public float maxPitch = 1.2f;  // Maximum pitch value

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playOnStart)
        {
            PlayRandomAudioClip();
        }
    }

    void PlayRandomAudioClip()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned.");
            return;
        }

        // Randomly select an audio clip from the array
        AudioClip selectedClip = audioClips[Random.Range(0, audioClips.Length)];

        // Randomly select a pitch within the specified range
        float randomPitch = Random.Range(minPitch, maxPitch);

        // Apply the pitch and play the selected clip
        audioSource.pitch = randomPitch;
        audioSource.clip = selectedClip;
        audioSource.Play();

        // Randomize the delay for the next clip
        float randomDelay = Random.Range(minDelay, maxDelay);

        // Schedule the next clip
        Invoke("PlayRandomAudioClip", randomDelay);
    }

    // Optional: Method to stop the sound playback
    public void StopAudio()
    {
        CancelInvoke("PlayRandomAudioClip");
        audioSource.Stop();
    }

    // Optional: Method to start playing sounds again
    public void StartAudio()
    {
        PlayRandomAudioClip();
    }
}
