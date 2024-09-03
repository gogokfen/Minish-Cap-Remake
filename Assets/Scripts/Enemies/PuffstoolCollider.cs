using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuffstoolCollider : MonoBehaviour
{
    [SerializeField] Puffstool puffstool;

    GameObject tempBody;

    float vulnerableWindup;

    private void Start()
    {
        tempBody = new GameObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" && !puffstool.dying)
        {
            if (puffstool.vulnerable)
            {
                Movement.swordHit = true;
                Vector3 tempDirection = (transform.position - Movement.playerPosition);
                tempDirection.Normalize();
                tempDirection *= 1.5f;
                puffstool.direction.x = tempDirection.x;
                puffstool.direction.y = tempDirection.z;


                puffstool.dying = true;
                puffstool.puffstoolBody.GetComponent<Renderer>().material.DOColor(new Color32(255, 0, 0, 255), 0.70f);
                puffstool.puffstoolLeg1.GetComponent<Renderer>().material.DOColor(new Color32(255, 0, 0, 255), 0.70f);
                puffstool.puffstoolLeg2.GetComponent<Renderer>().material.DOColor(new Color32(255, 0, 0, 255), 0.70f);
                SFXController.PlaySFX("PuffstoolHit", 0.2f);
                puffstool.gotHit = true;
            }
            else
            {
                Movement.swordBlocked = true;
                Movement.enemyShielded = true;
                Vector3 tempDirection2 = (Movement.playerPosition - transform.position);
                tempDirection2.Normalize();
                tempDirection2 *= 0.5f;
                Movement.SmallHit(new Vector2(tempDirection2.x,tempDirection2.z),true);
            }
        }

        if (other.tag == "Player" && !puffstool.vulnerable)
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection *= 1.5f;
            puffstool.direction.x = tempDirection.x;
            puffstool.direction.y = tempDirection.z;
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(puffstool.direction);
        }


        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection *= 1.5f;
            puffstool.direction.x = tempDirection.x;
            puffstool.direction.y = tempDirection.z;
            Movement.SmallHit(puffstool.direction);


            puffstool.gotHit = true;
            tempDirection = (transform.position - Movement.playerPosition);
            tempDirection.Normalize();
            tempDirection *= 1.5f;
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
            Movement.dustSucced = true;

            puffstool.doSpores = false;

            puffstool.anim.SetBool("Sucked", true);
            puffstool.anim.SetBool("Walk", false);
            puffstool.anim.SetBool("Jump", false);

            vulnerableWindup += Time.deltaTime;
            puffstool.stunned = true;

            tempBody.transform.position = Movement.playerPosition;
            puffstool.transform.LookAt(tempBody.transform.position); // not sure how else to make puffstool look at link without a serialize field reference

            if (vulnerableWindup >= 2)
            {
                if (!puffstool.whitend)
                {
                    puffstool.anim.SetBool("Sucked", false);
                    puffstool.anim.SetBool("Confused", true);

                    puffstool.stunEffect.SetActive(true);
                    puffstool.whitend = true;

                    puffstool.puffstoolBody.GetComponent<Renderer>().material = puffstool.whiteMaterial;
                }
                puffstool.vulnerable = true;
                puffstool.stunDuration = 10;
            }
            else
            {
                puffstool.stunDuration = 0.5f;
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
