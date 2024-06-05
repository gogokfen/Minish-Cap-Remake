using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slug : MonoBehaviour
{
    [SerializeField] bool fallingFromSky = false;
    [SerializeField] float fallingTimer;
    private float maxFall;
    [SerializeField] float moveSpeed;
    float rotationTimer;
    float animationTime;
    int rotation;

    [SerializeField] public int hp;
    [SerializeField] public TextMeshPro enemyText;

    [SerializeField] GameObject slugShadow;

    public bool gotHit;
    float gotHitTimer;
    public Vector2 direction;

    private void Start()
    {
        maxFall = fallingTimer;
    }

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
            {
                fallingFromSky = false;
            }

            if (fallingTimer>2f)
            {
                Color shadow = new Color(0, 0, 0, Mathf.InverseLerp(maxFall - 2.5f, 0, fallingTimer - 2.5f));
                slugShadow.GetComponent<Renderer>().material.SetColor("_BaseColor", shadow);
            }
            else
                slugShadow.SetActive(false);




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
}
