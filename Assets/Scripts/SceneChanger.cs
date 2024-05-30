using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public Button yourButton;
    public AudioClip soundClip;
    public string sceneName;
    public Animator loadingAnimator;
    public Image loadingSprite;

    [SerializeField] private HoverActivator hoverActivator;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
        yourButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        hoverActivator.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(PlaySoundAndLoadScene());
        loadingAnimator.SetBool("Reverse", true);
        loadingAnimator.Play("LeavesIntro");
        StartCoroutine(FadeInSprite());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator FadeInSprite()
    {
        Color spriteColor = loadingSprite.color;
        float fadeDuration = 1.0f;
        float fadeSpeed = 1.0f / fadeDuration;

        while (spriteColor.a < 1.0f)
        {
            spriteColor.a += fadeSpeed * Time.deltaTime;
            loadingSprite.color = spriteColor;
            yield return null;
        }
        spriteColor.a = 1.0f;
        loadingSprite.color = spriteColor;
    }

    public void OsherScene()
    {
        SceneManager.LoadScene("OsherScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
