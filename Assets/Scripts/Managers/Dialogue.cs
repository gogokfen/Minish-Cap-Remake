using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject dialogueBox;
    [TextArea(3, 10)]
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool dialogueOpen;
    private float timer;
    private static Dialogue instance;
    private bool typing;
    public GameObject lastSaysBox;
    public TextMeshProUGUI lastSaysText;

    void Start()
    {
        instance = this;
        text.text = string.Empty;
    }

    void Update()
    {
        if (dialogueOpen)
        {
            timer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && timer >= 1)
            {
                if (typing)
                {
                    StopAllCoroutines();
                    text.text = lines[index];
                    typing = false;
                }
                else
                {
                    CloseDialogue();
                    dialogueOpen = false;
                    Movement.BarrelRiding(false);
                }

                timer = 0;
                lastSaysBox.SetActive(true);
                lastSaysText.text = lines[index];
            }
        }
    }

    IEnumerator TypeLine()
    {
        typing = true;
        text.text = string.Empty;
        int charIndex = 0;

        while (charIndex < lines[index].Length)
        {
            if (!dialogueOpen)
            {
                break;
            }

            if (lines[index].Substring(charIndex).StartsWith("<sprite"))
            {
                int endIndex = lines[index].IndexOf(">", charIndex) + 1;
                string spriteTag = lines[index].Substring(charIndex, endIndex - charIndex);
                text.text += spriteTag;
                charIndex = endIndex;
                SFXController.PlaySFX("DialogueBlip", 0.5f);
            }
            else
            {
                text.text += lines[index][charIndex];
                charIndex++;
                SFXController.PlaySFX("DialogueBlip", 0.5f);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        typing = false;
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
