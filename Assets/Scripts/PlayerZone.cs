using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    public bool inZone;
    public bool immoveable;


    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            inZone = true;

        if (other.tag == "Moveable")
            immoveable = true;

    }
    */

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            inZone = true;

        if (other.tag == "Moveable")
        {
            immoveable = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            inZone = false;
        if (other.tag == "Moveable")
            immoveable = false;
    }
}
