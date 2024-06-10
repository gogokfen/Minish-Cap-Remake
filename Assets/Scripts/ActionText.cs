using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;

    private static string tempText;
    private static ActionText instance;
    private bool fadedIn;
    //public bool needsFading;

    [SerializeField] GameObject textOnObject;

    [SerializeField] TextMeshProUGUI textObject;

    static Vector3 pos;
    static float YRotation;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        tempText = "";
    }

    void Update()
    {
        //transform.position = pos;
        //text.transform.position = pos;
        //text.rectTransform.position = pos;

        text.text = tempText;
        textObject.text = tempText;

        if (text.text == "")
        {
            textOnObject.SetActive(false);
        }
        else
            textOnObject.SetActive(true);

    }

    public static void UpdateText(string newText)
    {
        tempText = newText;
        if ((tempText != ("") || tempText != ("Throw")) && instance.fadedIn == false)
        {
            instance.StartCoroutine(instance.FadeIn());
            instance.fadedIn = true;
        }
        else if (tempText == ("") && instance.fadedIn == true)
        {
            instance.StartCoroutine(instance.FadeOut());
            instance.fadedIn = false;
        }    
        // else
        // {
        //     instance.StartCoroutine(instance.FadeOut());
        // }
        // if (instance != null && instance.canvasGroup != null)
        // {
        //     if (!instance.fadedIn && instance.needsFading)
        //     {
        //         instance.StartCoroutine(instance.FadeIn());
        //     }
        //     else
        //     {
        //         instance.StartCoroutine(instance.FadeOut());
        //     }
        // }
        // else
        // {
        //     Debug.LogWarning("Instance of ActionText or CanvasGroup is not set.");
        // }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            }
            yield return null;
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            }
            yield return null;
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

}
