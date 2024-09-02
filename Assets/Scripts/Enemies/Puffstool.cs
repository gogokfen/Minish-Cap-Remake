using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Puffstool : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;
    float moveTimer;
    float rotation;
    [HideInInspector]
    public bool doSpores;
    bool pooped;
    float sporesTimer;
    float dyingTimer  = 0.75f;

    [HideInInspector]
    public bool gotHit;
    [HideInInspector]
    public float gotHitTimer;
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool vulnerable;
    [HideInInspector]
    public bool stunned;
    [HideInInspector]
    public float stunDuration;

    [SerializeField] GameObject sporePrefab;

    public GameObject puffstoolBody;
    public GameObject puffstoolLeg1;
    public GameObject puffstoolLeg2;
    [HideInInspector]
    public Material puffstoolMat;

    [HideInInspector]
    public bool whitend = false;

    [HideInInspector]
    public bool dying = false;

    public ParticleSystem dyingEffect;

    public GameObject stunEffect;

    public Animator anim;

    public Material originalMaterial;
    public Material whiteMaterial;

    [SerializeField] GameObject heartDropPrefab;

    void Start()
    {
        puffstoolMat = puffstoolBody.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (!stunned && !dying)
        {
            if (!doSpores)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                moveTimer += Time.deltaTime;

                anim.SetBool("Walk", true);
            }


            if (moveTimer > 1f && !doSpores)
            {
                moveTimer = 0;
                if (Random.Range(0, 4) == 0)
                {
                    rotation = Random.Range(0f, 360f);

                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rotation, transform.eulerAngles.z);
                }

                if (Random.Range(0, 12) == 0) //(0, 8)
                {
                    anim.SetBool("Walk", false);
                    doSpores = true;
                }


            }
            else if (doSpores)
            {
                sporesTimer += Time.deltaTime;

                if (sporesTimer>=1)
                {
                    anim.SetBool("Jump", true);
                }


                if (sporesTimer >= 2 && !pooped)
                {
                    Instantiate(sporePrefab, new Vector3(transform.position.x, 0.20f , transform.position.z), Quaternion.identity); //transform.position.y-0.24f 0.22f
                    pooped = true;
                }

                if (sporesTimer >= 4)
                {
                    sporesTimer = 0;
                    doSpores = false;
                    pooped = false;

                    anim.SetBool("Walk", true);
                    anim.SetBool("Jump", false);
                }

            }
        }
        else
        {
            stunDuration -= Time.deltaTime;
            if (stunDuration<=0)
            {
                stunEffect.SetActive(false);
                stunned = false;
                vulnerable = false;
                whitend = false;

                puffstoolBody.GetComponent<Renderer>().material = originalMaterial;

                anim.SetBool("Sucked", false);
                anim.SetBool("Confused", false);
                anim.SetBool("Walk", true);
            }
        }

        if (dying)
        {
            dyingTimer -= Time.deltaTime;
            if (dyingTimer<=0)
            {
                dying = false;
                Die();
            }
        }

        if (gotHit)
        {
            gotHit = false;
            gotHitTimer = 0.15f;
            if (vulnerable)
                Movement.swordHit = true;
        }

        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x + (direction.x * Time.deltaTime * 5), transform.position.y, transform.position.z + (direction.y * Time.deltaTime * 5)); //originally *2 and not timedeltatime
        }
    }

    public void Die()
    {
        if (Random.Range(1, HealthSystem.currentHealth) == 1)
        {
            Instantiate(heartDropPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
        }

        dyingEffect.transform.SetParent(null);
        dyingEffect.transform.rotation = Quaternion.identity;
        dyingEffect.transform.localScale = Vector3.one;
        dyingEffect.Play();
        SFXController.PlaySFX("EnemyPoof", 0.4f);
        Destroy(gameObject);
        Destroy(dyingEffect.gameObject, 3);
    }
}
