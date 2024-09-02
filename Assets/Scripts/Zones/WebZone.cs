using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebZone : MonoBehaviour
{
    float suctionWindup;

    bool dying = false;
    float dyingTimer = 0;

    Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (dying)
        {
            dyingTimer += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(originalPosition, new Vector3(Movement.gustJarPos.x, Movement.gustJarPos.y + 0.5f, Movement.gustJarPos.z), dyingTimer); //making it go to the gust jar
            transform.localScale /= (1 + Time.deltaTime * 8);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Shield")
        {
            SFXController.PlaySFX("SwordBonk", 0.5f);

            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection /= 1.5f;
            Movement.StopRolling();
            Movement.SmallHit(new Vector2 (tempDirection.x,tempDirection.z));  
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection /= 1.5f;
            Movement.StopRolling();
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
        }

        if (other.tag == "GustJar" && !dying)
        {
            suctionWindup += Time.deltaTime;
            if (suctionWindup > 0.5f)
            {
                dying = true;
                SFXController.PlaySFX("GustJarThump", 0.5f);
                Destroy(gameObject,0.5f);
            }
        }
    }
}
