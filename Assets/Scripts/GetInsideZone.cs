using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInsideZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        other.transform.position = transform.position; //collider layers of the object this is on do not interact with the player
    }
}
