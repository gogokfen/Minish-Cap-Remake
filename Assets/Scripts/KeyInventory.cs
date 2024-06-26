using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyInventory : MonoBehaviour
{
    public static int Key = 0;
    public static int bossKey = 0;
    private static bool changeKey;
    [SerializeField] TextMeshProUGUI keyText;

    private void Start()
    {
        Key = 0;
    }
    private void Update() 
    {
        if (changeKey)
        {
        keyText.text = Key.ToString();
        changeKey = false;
        }
    }


    public static void AddKey()
    {
        Key++;
        changeKey = true;
    }

    public static void RemoveKey()
    {
        Key--;
        changeKey = true;
    }
}
