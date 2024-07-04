using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Slug : MonoBehaviour
{
    [SerializeField] bool fallingFromSky = false;
    [SerializeField] float fallingTimer;
    private float maxFall;
    [SerializeField] float moveSpeed;
    float randomizer;
    float rotRandomizer;
    float originalSpeed;
    float rotationTimer;
    float animationTime;
    int rotation;
    bool idle;
    Quaternion prevRot;

    public int hp;
    //public TextMeshPro enemyText;

    [SerializeField] GameObject slugShadow;

    [HideInInspector]
    public bool gotHit;
    float gotHitTimer;
    [HideInInspector]
    public Vector2 direction;

    [SerializeField] GameObject slugBody;
    Material slugMat;
    Color32 slugColor;

    public ParticleSystem hitEffect;

    public ParticleSystem trail;

    public Transform parent;

    Tween slugDrop;

    private void Start()
    {
        randomizer = Random.Range(0, 10f);
        maxFall = fallingTimer;
        slugMat = slugBody.GetComponent<Renderer>().material;
        slugColor = new Color32(255, 250, 146, 255);
        slugMat.SetColor("_BaseColor", slugColor);

        originalSpeed = moveSpeed;

        if (fallingFromSky)
        {
            slugShadow.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(0, 0, 0, 0));
            slugDrop.SetDelay(2);
            slugDrop = slugShadow.GetComponent<Renderer>().material.DOColor(new Color(0, 0, 0, 1), 2);


        }

    }

    void Update()
    {
        //enemyText.text = "hp: " + hp;
        if (!fallingFromSky)
        {
            moveSpeed = 0.2f + originalSpeed * Mathf.Abs(Mathf.Sin(((Time.time / 1.5f) + randomizer + rotRandomizer))) * Mathf.Pow(((Time.time / 1.5f) + randomizer + rotRandomizer) % Mathf.PI / 2f, 3); //riz cooked? base move speed 0.2f? in inspector 1.75?

            //Debug.Log(moveSpeed);
            //moveSpeed = originalSpeed* Mathf.Abs(Mathf.Sin((Time.time+randomizer)))*Mathf.Pow((Time.time+randomizer)%Mathf.PI/2f,2); //riz cooked?
            rotationTimer -= Time.deltaTime;
            animationTime += Time.deltaTime / 2; //*4
            //transform.rotation = Quaternion.Lerp(prevRot, Quaternion.Euler(0, rotation * 45, 0), animationTime);
            transform.rotation = Quaternion.Lerp(prevRot, Quaternion.Euler(0, prevRot.eulerAngles.y + rotation, 0), animationTime); //makes more sense he can only rotate+-180 degress from his current rotation

            if (rotationTimer <= 0)
            {
                animationTime = 0;
                prevRot = transform.rotation;
                rotRandomizer = Random.Range(-1, 2);
                rotationTimer = 4 + rotRandomizer; // + Random.Range(-1,1);
                //rotation = Random.Range(-2, 3); //(1,9)
                rotation = Random.Range(-90, 91);
                if (Random.Range(0, 4) == 0)
                {
                    idle = true;
                    rotation = 0;
                }
                else
                    idle = false;
                //transform.DORotate(new Vector3(0, rotation * 45, 0), 1);
                //transform.DORotateQuaternion(Quaternion.Euler(0, rotation * 45, 0), 1);
            }
            if (gotHitTimer <= 0 && !idle)
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        }
        else
        {
            fallingTimer -= Time.deltaTime;
            if (fallingTimer <= 0)
            {
                fallingFromSky = false;
            }

            if (fallingTimer > 2f)
            {
                //Color shadow = new Color(0, 0, 0, Mathf.InverseLerp(maxFall - 2.5f, 0, fallingTimer - 2.5f));
                //slugShadow.GetComponent<Renderer>().material.SetColor("_BaseColor", shadow);
            }
            else
                slugShadow.SetActive(false);
        }

        if (gotHit)
        {
            gotHit = false;
            gotHitTimer = 0.4f;
            Sequence slugHit = DOTween.Sequence();
            slugHit.Append(slugMat.DOColor(new Color32(255, 125, 146, 255), 0.25f));
            slugHit.Append(slugMat.DOColor(new Color32(255, 255, 146, 255), 0.25f));
        }

        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;

            if (gotHitTimer > 0.25f)
                transform.position = new Vector3(transform.position.x + (direction.x * Time.deltaTime * 3), transform.position.y, transform.position.z + (direction.y * Time.deltaTime * 3)); //originally *2 and not timedeltatime

            //slugMat.SetColor("_BaseColor", slugColor);


            /*
            if (gotHitTimer > 0.25f)
            {
                slugColor = new Color32(255, (byte)(0 + (gotHitTimer * 650)), 146, 255);
                transform.position = new Vector3(transform.position.x + (direction.x * Time.deltaTime * 3), transform.position.y, transform.position.z + (direction.y * Time.deltaTime * 3)); //originally *2 and not timedeltatime
            }
            else
            {
                slugColor = new Color32(255, (byte)(125+ ((0.25-gotHitTimer) * 500)), 146, 255);
            }
            */
        }
    }
}
