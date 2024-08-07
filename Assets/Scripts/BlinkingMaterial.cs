using UnityEngine;

public class BlinkingMaterial : MonoBehaviour
{
    public float startBlinkingDelay = 2f;
    public float blinkDuration = 5f;
    public float blinkInterval = 0.5f;
    private bool isBlinking = false;
    private Renderer childRenderer;

    private void Start()
    {

        childRenderer = GetComponentInChildren<Renderer>();
        if (childRenderer == null)
        {
            Debug.LogError("No Renderer found in children. Make sure to attach the script to the parent object with a child containing the material to blink.");
            return;
        }


        Invoke("StartBlinking", startBlinkingDelay);
    }

    private void StartBlinking()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            InvokeRepeating("ToggleMaterial", 0f, blinkInterval);

            Invoke("DestroyAfterBlinking", blinkDuration);
        }
    }

    private void ToggleMaterial()
    {
        childRenderer.enabled = !childRenderer.enabled;
    }

    private void DestroyAfterBlinking()
    {
        Destroy(gameObject);
    }
}