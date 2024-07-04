using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugCollider : MonoBehaviour
{
    [SerializeField] Slug slug;

    bool dying = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Weapon") && !dying)
        {
            slug.hitEffect.Play();
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
                    dying = true;
                    Destroy(slug.gameObject,0.25f);
                }
                //SFXController.PlaySFX("SlugDie", 0.55f); //Play slug getting hit SFX
            }
            slug.gotHit = true;
            //slug.enemyText.text = "Hp: " + slug.hp;
        }


        if (other.tag.Equals("Player") && !dying)
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            slug.direction.x = tempDirection.x;
            slug.direction.y = tempDirection.z;
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(slug.direction);
        }


        if (other.tag.Equals("Shield") && !dying)
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
