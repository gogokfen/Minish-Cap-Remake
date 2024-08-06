using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorChangingImage : MonoBehaviour
{
    public Image image; // Reference to the Image component
    public Color color1 = Color.red; // First color
    public Color color2 = Color.blue; // Second color
    public float duration = 1.0f; // Duration of the color change
    public Color startingColor = new Color(99f / 255f, 49f / 255f, 198f / 255f); // Starting color

    private Coroutine colorChangeCoroutine;

    private void OnEnable()
    {
        if (image != null)
        {
            colorChangeCoroutine = StartCoroutine(ChangeColor());
        }
    }

    private void OnDisable()
    {
        if (colorChangeCoroutine != null)
        {
            image.color = startingColor;
            StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = null;
        }

        // Reset the image color to the starting color
        if (image != null)
        {
            image.color = startingColor;
        }
    }

    IEnumerator ChangeColor()
    {
        float t = 0;
        while (true)
        {
            // Gradually change from color1 to color2 over the duration
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                image.color = Color.Lerp(color1, color2, t / duration);
                yield return null;
            }

            // Swap colors
            Color temp = color1;
            color1 = color2;
            color2 = temp;

            // Reset the time counter
            t = 0;
        }
    }
}
