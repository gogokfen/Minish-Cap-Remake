using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugCollider : MonoBehaviour
{
    [SerializeField] Slug slug;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            if (!slug.gotHit)
            {
                slug.hp--;
            }
            if (slug.hp<= 0)
            {
                Destroy(slug.gameObject);
            }

            slug.gotHit = true;
            slug.enemyText.text = "Hp: " + slug.hp;
        }


        if (other.tag == "Player")
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(slug.direction);
        }


        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            Movement.SmallHit(slug.direction);

            slug.gotHit = true;
            tempDirection = (transform.position - Movement.playerPosition);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;

        }
    }
}
