using System.Collections;
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

    private static Dialogue instance;
    void Start()
    {
        instance = this;
        text.text = string.Empty;
    }

    void Update()
    {
        if (dialogueOpen && Input.GetKeyDown(KeyCode.Space))
        {
            CloseDialogue();
            dialogueOpen = false;
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public static void StartDialogue(int lineIndex)
    {
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
