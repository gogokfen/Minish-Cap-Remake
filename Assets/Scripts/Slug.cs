using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slug : MonoBehaviour
{
    [SerializeField] bool fallingFromSky = false;
    [SerializeField] float fallingTimer;
    [SerializeField] float moveSpeed;
    float rotationTimer;
    float animationTime;
    int rotation;

    [SerializeField] int hp;
    [SerializeField] TextMeshPro enemyText;
    bool gotHit;
    float gotHitTimer;
    Vector2 direction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyText.text = "hp: " + hp;
        if (!fallingFromSky)
        {
            rotationTimer -= Time.deltaTime;
            animationTime += Time.deltaTime; //*4
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation * 90, 0), animationTime);
            if (rotationTimer<=0)
            {
                animationTime = 0;
                rotationTimer = 4;
                rotation = Random.Range(1, 5);
            }
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            fallingTimer -= Time.deltaTime;
            if (fallingTimer <= 0)
                fallingFromSky = false;
        }

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
        if (other.tag == "Weapon")
        {
            Vector3 tempDirection = (transform.position - Movement.playerPosition);
            direction.x = tempDirection.x;
            direction.y = tempDirection.z;
            if (!gotHit)
            {
                hp--;
            }
            if (hp<=0)
            {
                Destroy(gameObject);
            }

            gotHit = true;
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
            Movement.SmallHit(direction);

            gotHit = true;
            tempDirection = (transform.position - Movement.playerPosition);
            direction.x = tempDirection.x;
            direction.y = tempDirection.z;

        }
    }
}
