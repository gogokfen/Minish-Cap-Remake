using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidTempOsherButton : MonoBehaviour
{
    [SerializeField] GameObject osherButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Z))
        {
            osherButton.SetActive(true);
        }
    }
}
