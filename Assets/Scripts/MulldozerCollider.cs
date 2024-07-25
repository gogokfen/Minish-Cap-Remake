using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulldozerCollider : MonoBehaviour
{
    [SerializeField] Mulldozer mulldozer;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Moveable"))
        {
            mulldozer.transform.Rotate(0, 180, 0);
        }

        if (other.tag == "Weapon" && !mulldozer.dying) // && !puffstool.dying
        {
            mulldozer.StopAttacking(); //attacking mulldozer stops him from attacking

            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            mulldozer.direction.x = tempDirection.x;
            mulldozer.direction.y = tempDirection.z;
            mulldozer.hp--;
            if (mulldozer.hp<=0)
            {

                mulldozer.dying = true;
                //puffstool.puffstoolMat.DOColor(new Color32(255, 0, 0, 255), 0.70f);
                //mulldozer.Die();
                //Destroy(mulldozer.gameObject,0.75f); 
            }

            mulldozer.gotHit = true;

            
            mulldozer.stunned = false;
            mulldozer.stunnedEffect.SetActive(false);
        }

        if (other.tag.Equals("Player"))
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.enemyHitAmount = mulldozer.damage;
            tempDirection /= 1.25f; //reducing knockback amount
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
        }

        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));


            tempDirection = (transform.position - Movement.playerPosition);
            tempDirection *= 3;
            mulldozer.direction.x = tempDirection.x;
            mulldozer.direction.y = tempDirection.z;
            mulldozer.gotHit = true;

            mulldozer.stunnedEffect.SetActive(true);
            mulldozer.StopAttacking();
            mulldozer.stunned = true;
            mulldozer.stunTimer = 3f;
        }
    }
}
