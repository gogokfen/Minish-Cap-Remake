using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudZone : MonoBehaviour
{
    float suctionWindup;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Movement.mud = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar")
        {
            suctionWindup += Time.deltaTime;
            if (suctionWindup>0.5f)
            {
                Movement.dustSucced = true;
                Destroy(gameObject);
            }
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
