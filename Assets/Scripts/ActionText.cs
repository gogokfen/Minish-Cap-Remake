using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionText : MonoBehaviour
{
    TextMeshProUGUI text;

    static string tempText;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = tempText;
    }

    public static void UpdateText(string newText)
    {
        tempText = newText;
    }
}
