using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnUiForAlpha : MonoBehaviour
{
    [SerializeField] GameObject UI;
    void Start()
    {
        //UI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Chest.gotGotJar && !UI.activeSelf)
        {
            UI.SetActive(true);
        }
    }
}
