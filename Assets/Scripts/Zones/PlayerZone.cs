using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour //alt version of Zone script, exlcusive to the player
{
    public bool inZone;
    public bool immoveable;

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
