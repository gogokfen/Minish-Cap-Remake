using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection *= 1.5f;
            Movement.Stun(1);
            Movement.playerPosition = new Vector3(Movement.playerPosition.x + tempDirection.x, Movement.playerPosition.y+0.75f, Movement.playerPosition.z + tempDirection.z);

        }
    }
}
