using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebZone : MonoBehaviour
{
    float suctionWindup;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.SmallHit(new Vector2 (tempDirection.x,tempDirection.z));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar")
        {
            suctionWindup += Time.deltaTime;
            if (suctionWindup > 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}
