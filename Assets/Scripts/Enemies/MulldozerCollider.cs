using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulldozerCollider : MonoBehaviour
{
    [SerializeField] Mulldozer mulldozer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Moveable"))
        {
            mulldozer.transform.Rotate(0, 180, 0);
        }

        if (other.tag == "Weapon" && !mulldozer.dying) // && !puffstool.dying
        {
            mulldozer.StopAttacking(); //attacking mulldozer stops him from attacking
            SFXController.PlaySFX("MulldozerHit", 0.5f);
            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            tempDirection.Normalize();
            tempDirection *= 2f;

            mulldozer.rigid.AddForce(new Vector3(tempDirection.x * 10, 0, tempDirection.z * 10), ForceMode.Impulse);

            mulldozer.hp--;
            if (mulldozer.hp<=0)
            {
                mulldozer.dying = true;
            }

            mulldozer.gotHit = true;

            mulldozer.stunned = false;
            mulldozer.stunnedEffect.SetActive(false);
        }

        else if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection *= 1.5f;
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
            SFXController.PlaySFX("MulldozerShield", 0.5f);
            tempDirection = (transform.position - Movement.playerPosition);
            tempDirection.Normalize();
            tempDirection *= 3;
            mulldozer.direction.x = tempDirection.x;
            mulldozer.direction.y = tempDirection.z;
            mulldozer.gotHit = true;

            mulldozer.rigid.AddForce(new Vector3(tempDirection.x * 10, 0, tempDirection.z * 10), ForceMode.Impulse);

            mulldozer.stunnedEffect.SetActive(true);
            mulldozer.StopAttacking();
            mulldozer.stunned = true;
            mulldozer.stunTimer = 3f;
        }

        else if (other.tag.Equals("Player") && !Movement.enemyShielded) //making sure he wasn't blocked by a shield simutaionsly
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.enemyHitAmount = mulldozer.damage;
            tempDirection.Normalize();
            tempDirection *= 1.5f;
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
        }
    }
}
