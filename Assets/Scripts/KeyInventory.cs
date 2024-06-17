using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyInventory : MonoBehaviour
{
    public static int Key = 0;
    public static int bossKey = 0;
    private static bool addingKey;
    [SerializeField] TextMeshProUGUI keyText;

    private void Start()
    {
        Key = 0;
    }
    private void Update() 
    {
        if (addingKey)
        {
        keyText.text = Key.ToString();
        addingKey = false;
        }
    }


    public static void AddKey()
    {
        Key++;
        addingKey = true;
    }
}
