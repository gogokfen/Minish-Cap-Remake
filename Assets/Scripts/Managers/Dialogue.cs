using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject dialogueBox;
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool dialogueOpen;
    float timer;
    private static Dialogue instance;
    void Start()
    {
        instance = this;
        text.text = string.Empty;
    }

    void Update()
    {
        if (dialogueOpen)
        {
        timer = timer + Time.deltaTime;
        }
        if (dialogueOpen && Input.GetKeyDown(KeyCode.Space) && timer >= 1)
        {
            CloseDialogue();
            dialogueOpen = false;
            Movement.BarrelRiding(false);
            timer = 0;
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            if (!dialogueOpen)
            {
                break;
            }
            text.text += c;
            int randomSFX = Random.Range(1, 8);
            switch (randomSFX)
            {
                case 1:
                    SFXController.PlaySFX("EzioCaw1", 0.55f);
                    break;
                case 2:
                    SFXController.PlaySFX("EzioCaw2", 0.55f);
                    break;
                case 3:
                    SFXController.PlaySFX("EzioCaw3", 0.55f);
                    break;
                case 4:
                    SFXController.PlaySFX("EzioCaw4", 0.55f);
                    break;
                case 5:
                    SFXController.PlaySFX("EzioCaw5", 0.55f);
                    break;
                case 6:
                    SFXController.PlaySFX("EzioCaw6", 0.55f);
                    break;
                case 7:
                    SFXController.PlaySFX("EzioCaw7", 0.55f);
                    break;
            }
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public static void StartDialogue(int lineIndex)
    {
        Movement.BarrelRiding(true, true);
        instance.index = lineIndex;
        instance.dialogueBox.SetActive(true);
        instance.dialogueOpen = true;
        instance.text.text = string.Empty;
        instance.StartCoroutine(instance.TypeLine());  
    }

    void CloseDialogue()
    {
        text.text = string.Empty;
        dialogueBox.SetActive(false);
    }
}
