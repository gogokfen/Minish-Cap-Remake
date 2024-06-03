using UnityEngine;
public class SFXController : MonoBehaviour
{
    private static SFXController instance;
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static void PlaySFX(string clipName, float volume = 1.0f)
    {
        if (instance != null && instance.audioSource != null && !string.IsNullOrEmpty(clipName))
        {
            
            string path = $"Sounds/Sfx/{clipName}";
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                instance.audioSource.PlayOneShot(clip, volume);
            }
            else
            {
                Debug.Log($"AudioClip '{clipName}' not found at '{path}' in Resources.");
            }
        }
        else
        {
            Debug.Log("SFXController or AudioSource or clipName is missing.");
        }
    }
}
