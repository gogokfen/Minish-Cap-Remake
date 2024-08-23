using UnityEngine;
using UnityEngine.Events;
public class BarrelRoom : MonoBehaviour
{
    private int buttonsClicked = 0;
    private int leftClicked = 0;
    private bool sideComplete;
    private float timer;
    [SerializeField] UnityEvent barrelDropEvent;
    [SerializeField] PressureButton pressureButton1;
    [SerializeField] PressureButton pressureButton2;
    [SerializeField] Animator leftBrazier;
    [SerializeField] Animator rightBrazier;

    private void Update()
    {
        if (sideComplete)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 5)
        {
            buttonsClicked++;
            timer = 0f;
            sideComplete = false;
            if (buttonsClicked == 1)
            {
                SFXController.PlaySFX("Secret", 0.5f);
                Dialogue.StartDialogue(3);
            }
            DropTheBarrel();
        }
    }

    public void FinishRight()
    {
        Movement.Scene(5);
        rightBrazier.Play("RightBrazier");
        sideComplete = true;
    }
    public void FinishLeft()
    {
        leftClicked++;
        if (leftClicked == 2)
        {
            Destroy(pressureButton1);
            Destroy(pressureButton2);
            sideComplete = true;
            Movement.Scene(5);
            leftBrazier.Play("LeftBrazier");
        }
    }
    public void DropTheBarrel()
    {
        if (buttonsClicked == 2)
        {
            barrelDropEvent.Invoke();
            Dialogue.StartDialogue(4);
            SFXController.PlaySFX("Secret", 0.5f);
            Destroy(this);
        }
    }
    public void ButtonUp()
    {
        leftClicked--;
    }
}
