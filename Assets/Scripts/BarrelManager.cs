using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    [SerializeField] GameObject barrelCam;

    private void OnTriggerEnter(Collider other) 
    {
        barrelCam.SetActive(true);
    }

    private void OnTriggerExit(Collider other) 
    {
        barrelCam.SetActive(false);
    }
}
