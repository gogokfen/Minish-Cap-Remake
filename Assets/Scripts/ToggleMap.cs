using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMap : MonoBehaviour
{
    public GameObject mapUI;
    private bool mapOpen = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapOpen = !mapOpen;
            mapUI.SetActive(mapOpen);
        }

        if (mapOpen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
