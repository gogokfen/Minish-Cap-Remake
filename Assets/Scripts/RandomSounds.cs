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

    [Header("SphereCast Settings")]
    public float sphereRadius = 5f;
    public LayerMask mask;

    private AudioSource audioSource;
    private bool isPlayingRandomClip = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playOnStart)
        {
            PlayRandomAudioClip();
        }
    }

    private void Update() 
    {
        if (Physics.CheckSphere(transform.position, sphereRadius, mask))
        {
            if (!isPlayingRandomClip)
            {
                PlayRandomAudioClip();
            }
        }
        else
        {
            StopAudio();
        }
    }

    void PlayRandomAudioClip()
    {
        isPlayingRandomClip = true;

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

        // Schedule the next clip and reset the flag
        Invoke("ResetIsPlayingFlag", selectedClip.length + randomDelay);
    }

    void ResetIsPlayingFlag()
    {
        isPlayingRandomClip = false;
    }

    // Optional: Method to stop the sound playback
    public void StopAudio()
    {
        CancelInvoke("PlayRandomAudioClip");
        isPlayingRandomClip = false;
        audioSource.Stop();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }

    public void AnimationSound(AudioClip clip)
    {
        if (Physics.CheckSphere(transform.position, sphereRadius, mask))
        {
            CancelInvoke("PlayRandomAudioClip");
            isPlayingRandomClip = false;
            audioSource.clip = clip;
            audioSource.PlayOneShot(clip);
        }
    }
}
