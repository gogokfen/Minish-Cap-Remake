using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffstoolCollider : MonoBehaviour
{
    [SerializeField] Puffstool puffstool;

    float vulnerableWindup;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            puffstool.direction.x = tempDirection.x;
            puffstool.direction.y = tempDirection.z;
            if (puffstool.vulnerable)
            {
                Destroy(puffstool.gameObject);
            }

            puffstool.gotHit = true;
        }


        if (other.tag == "Player")
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            puffstool.direction.x = tempDirection.x;
            puffstool.direction.y = tempDirection.z;
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(puffstool.direction);
        }


        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            puffstool.direction.x = tempDirection.x;
            puffstool.direction.y = tempDirection.z;
            Movement.SmallHit(puffstool.direction);

            puffstool.gotHit = true;
            tempDirection = (transform.position - Movement.playerPosition);
            puffstool.direction.x = tempDirection.x;
            puffstool.direction.y = tempDirection.z;

        }

        if (other.tag == "GustJar")
        {
            vulnerableWindup = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar")
        {
            vulnerableWindup += Time.deltaTime;
            puffstool.stunned = true;

            if (vulnerableWindup>=2)
            {
                puffstool.vulnerable = true;
                puffstool.stunDuration = 10;
            }
            else
            {
                puffstool.stunDuration = 2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GustJar")
        {
            vulnerableWindup = 0;
        }
    }
}
