using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Movement.mud = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Movement.mud = false;
        }
    }
}
