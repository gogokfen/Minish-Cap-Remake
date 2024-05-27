using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.SmallHit(new Vector2 (tempDirection.x,tempDirection.z));
        }
    }
}
