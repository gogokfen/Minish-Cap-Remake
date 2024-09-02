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
    public static float textTimer = 0;

    private static ActionText instance;
    private bool fadedIn;

    [SerializeField] GameObject textOnObject;

    [SerializeField] TextMeshProUGUI textObject;


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
        text.text = tempText;
        textObject.text = tempText;

        if (textTimer>0)
        {
            textTimer -= Time.deltaTime;
        }

        if (text.text == "" || textTimer<=0)
        {
            textOnObject.SetActive(false);
        }
        else
            textOnObject.SetActive(true);

    }

    public static void UpdateText(string newText)
    {
        tempText = newText;
        textTimer = 3;


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
