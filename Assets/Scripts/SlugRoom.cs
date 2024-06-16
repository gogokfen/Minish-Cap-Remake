using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlugRoom : MonoBehaviour
{
    int pressureButtonCounter = 0;
    [SerializeField] UnityEvent slugRoomEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressureButton()
    {
        pressureButtonCounter++;
        if (pressureButtonCounter == 4)
        {
            slugRoomEvent.Invoke();
        }
    }
}
