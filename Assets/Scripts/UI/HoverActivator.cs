using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToActivate;
    public Sprite hoverSprite;
    private Sprite defaultSprite;
    private Image buttonImage;

    void Start()
    {
        // Get the Image component of the button
        buttonImage = GetComponent<Image>();

        // Store the default sprite
        if (buttonImage != null)
        {
            defaultSprite = buttonImage.sprite;
        }
    }

    // This method is called when the pointer enters the UI element this script is attached to
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Change to hover sprite
        if (buttonImage != null && hoverSprite != null)
        {
            buttonImage.sprite = hoverSprite;
        }
    }

    // This method is called when the pointer exits the UI element this script is attached to
    public void OnPointerExit(PointerEventData eventData)
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }

        // Revert to default sprite
        if (buttonImage != null && defaultSprite != null)
        {
            buttonImage.sprite = defaultSprite;
        }
    }
}
