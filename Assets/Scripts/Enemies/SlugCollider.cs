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
            // slug.transform.Rotate(0, 180, 0);
            slug.rotation = (int)(transform.eulerAngles.y + 180);
        }

        if (other.tag.Equals("Weapon") && !slug.dying)
        {
            Movement.swordHit = true;
            //slug.hitEffect.Play();
            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            if (!slug.gotHit)
            {
                slug.hp--;
                if (slug.hp <= 0)
                {
                    //SFXController.PlaySFX("SlugHit", 0.55f); //Play slug dying SFX
                    slug.trail.transform.SetParent(slug.parent);   
                    var main = slug.trail.main;
                    main.simulationSpeed = 3;

                    slug.trail.Stop();
                    //slug.trail.main.loop
                    slug.dying = true;
                    slug.BecomeRed();
                    //Destroy(slug.gameObject,0.25f);
                }
                //SFXController.PlaySFX("SlugDie", 0.55f); //Play slug getting hit SFX
            }
            slug.gotHit = true;
            slug.gameObject.layer = 8; //character layer
            //gameObject.layer = 8; 
            //slug.enemyText.text = "Hp: " + slug.hp;
        }


        if (other.tag.Equals("Player") && !slug.dying)
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(slug.direction);
        }


        if (other.tag.Equals("Shield") && !slug.dying)
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            Movement.SmallHit(slug.direction);

            slug.gotHit = true;
            tempDirection = (transform.position - Movement.playerPosition);
            tempDirection *= 2;
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;

            slug.gameObject.layer = 8; //character layer
            //gameObject.layer = 8; 
        }
    }
}
