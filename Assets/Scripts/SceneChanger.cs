using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{

    public Button yourButton;
    public AudioClip soundClip;
    public string sceneName;
    [SerializeField] HoverActivator hoverActivator;

    private AudioSource audioSource;

    void Start()
    {

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;


        yourButton.onClick.AddListener(PlaySoundAndChangeScene);
    }

    void PlaySoundAndChangeScene()
    {


        hoverActivator.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;


        StartCoroutine(PlaySoundAndChangeSceneCoroutine());
    }

    IEnumerator PlaySoundAndChangeSceneCoroutine()
    {

        audioSource.Play();


        yield return new WaitForSeconds(audioSource.clip.length);


        SceneManager.LoadScene(sceneName);
    }
}
