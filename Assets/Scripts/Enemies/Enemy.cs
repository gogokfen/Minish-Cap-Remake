using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] TextMeshPro enemyText;

    bool gotHit;
    float gotHitTimer;

    Vector2 direction;
    void Start()
    {
        //direction = -Vector2.up;
    }

    // Update is called once per frame
    void Update()
    {
        enemyText.text = "hp: " + hp;

        if (gotHit)
        {
            gotHit = false;
            gotHitTimer = 0.15f;
        }


        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x + (direction.x * Time.deltaTime * 8), transform.position.y, transform.position.z + (direction.y * Time.deltaTime * 8)); //originally *2 and not timedeltatime

        }

    }

    
    private void OnTriggerEnter(Collider other)
    {


        if(other.tag == "Weapon")
        {
            gotHit = true;
            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            direction.x = tempDirection.x;
            direction.y = tempDirection.z;

            hp--;
            enemyText.text = "Hp: " + hp;
        }

        
        if (other.tag == "Player")
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            direction.x = tempDirection.x;
            direction.y = tempDirection.z;
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(direction);
        }
  

        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            direction.x = tempDirection.x;
            direction.y = tempDirection.z;
            //HealthSystem.TakeDamage(1);
            Movement.SmallHit(direction);

            gotHit = true;
            Vector3 tempDirection2 = (transform.position - Movement.playerPosition);
            direction.x = tempDirection2.x;
            direction.y = tempDirection2.z;
            
        }
    }
    

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Weapon")
        {
            hp--;
            enemyText.text = "Hp: " + hp;
        }

        if (collision.transform.tag == "Player")
        {
            Movement.smallHit();
        }
    }
    */
}
