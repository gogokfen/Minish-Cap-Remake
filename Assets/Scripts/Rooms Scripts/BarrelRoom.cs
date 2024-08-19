using UnityEngine;
using UnityEngine.Events;
public class BarrelRoom : MonoBehaviour
{
    private int buttonsClicked = 0;
    private int rightClicked = 0;
    [SerializeField] UnityEvent barrelDropEvent;
    [SerializeField] PressureButton pressureButton1;
    [SerializeField] PressureButton pressureButton2;
    [SerializeField] GameObject brazierFlameRight;
    public void FinishRight()
    {
        buttonsClicked++;
        DropTheBarrel();
        // if (buttonsClicked == 3)
        // {
        //     //barrel.transform.position += new Vector3 (0, -3, 0);
        //     barrelDropEvent.Invoke();
        //     // barrelTriggerOne.SetActive(true);
        //     // barrelTriggerTwo.SetActive(true);
        //     Destroy(pressureButton1);
        //     Destroy(pressureButton2);
        //     SFXController.PlaySFX("Secret", 0.5f);
        //     Destroy(this);
        // }
    }
    public void FinishLeft()
    {
        rightClicked++;
        if (rightClicked == 2)
        {
            buttonsClicked++;
            brazierFlameRight.SetActive(true);
            Destroy(pressureButton1);
            Destroy(pressureButton2);
        }
        DropTheBarrel();
    }
    public void DropTheBarrel()
    {
        if (buttonsClicked == 2)
        {
            barrelDropEvent.Invoke();
            Dialogue.StartDialogue(3);
            SFXController.PlaySFX("Secret", 0.5f);
            Destroy(this);
        }
    }
    public void ButtonUp()
    {
        rightClicked--;
    }
}
