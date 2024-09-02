using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugCollider : MonoBehaviour
{
    [SerializeField] Slug slug;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Moveable"))
        {
            slug.rotation = (int)(transform.eulerAngles.y + 180); //preventing the slug from sticking to walls
        }

        if (other.tag.Equals("Weapon") && !slug.dying)
        {
            Movement.swordHit = true;
            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            tempDirection.Normalize();
            tempDirection *= 2.5f;
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            SFXController.PlaySFX("SluggulaHit", 0.4f);
            if (!slug.gotHit)
            {
                slug.hp--;
                if (slug.hp <= 0)
                {
                    slug.trail.transform.SetParent(slug.parent);   
                    var main = slug.trail.main;
                    main.simulationSpeed = 3;
                    
                    slug.trail.Stop();
                    slug.dying = true;
                    slug.BecomeRed();
                }
            }
            slug.gotHit = true;
            slug.gameObject.layer = 8; //character layer, make it possible for the slug to fall into water when knocked back
        }


        if (other.tag.Equals("Player") && !slug.dying)
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection *= 1.5f;
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(slug.direction);
        }


        if (other.tag.Equals("Shield") && !slug.dying)
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection *= 1.5f;
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            Movement.SmallHit(slug.direction);

            slug.gotHit = true;
            tempDirection = (transform.position - Movement.playerPosition);
            tempDirection.Normalize();
            tempDirection *= 5f;
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;

            slug.gameObject.layer = 8; //character layer
        }
    }
}
