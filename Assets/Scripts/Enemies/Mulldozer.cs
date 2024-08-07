using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mulldozer : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public int hp;
    float moveTimer;

    bool attacking;
    bool charging;
    float attackCooldown;
    [SerializeField] float updatedAttackCooldown = 3;
    float attackTimer;
    float rotationDirection = 90;
    float chargeRotationTimer = 0.15f; //0.25f

    [SerializeField] Vector3 detectionBox; // 0.5 ,0.5 0.5
    [SerializeField] float DetectionRange; //10
    public int damage;

    Vector3 originalPos;

    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool gotHit;
    float gotHitTimer;

    [HideInInspector]
    public bool stunned;
    [HideInInspector]
    public float stunTimer = 3;
    [HideInInspector]
    public bool dying = false;
    float dyingTimer = 0.75f;

    //Ray raycast;
    RaycastHit player;
    [SerializeField] LayerMask mask;

    [Header("VFX")]
    public ParticleSystem dyingEffect;

    public GameObject stunnedEffect;

    public ParticleSystem chargingEffect;

    public GameObject attackingEffect;

    [SerializeField] GameObject mulldozerBody;
    [HideInInspector]
    public Material mulldozerMat;

    [HideInInspector]
    public Rigidbody rigid;


    void Start()
    {
        mulldozerMat = mulldozerBody.GetComponent<Renderer>().material;
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dying)
        {
            if (!attacking && !stunned)
            {
                moveTimer -= Time.deltaTime;
                attackCooldown -= Time.deltaTime;
                if (moveTimer > 0.5f)
                {
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                }
                else if (moveTimer > 0)
                {
                    //idle
                }
                else
                {
                    moveTimer = Random.Range(0.5f, 2f);
                    transform.rotation = Quaternion.Euler(0, Random.Range(0, 8) * 45, 0); // Random.Range(0, 4) * 90 originally
                }

                /*
                if (Physics.Raycast(transform.position, transform.forward, out player, 10, mask)) //raycast in the direction he is looking
                {
                    if (attackCooldown<=0)
                    {
                        attacking = true;
                    }
                }
                */
                /**
                if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out player, Quaternion.identity, 10, mask)) //raycast in the direction he is looking
                {
                    if (attackCooldown <= 0)
                    {
                        chargingEffect.Play();
                        attacking = true;
                        originalPos = transform.position;
                    }
                }
                */
                if (Physics.BoxCast(transform.position, detectionBox, transform.forward, out player, Quaternion.identity, DetectionRange, mask)) //raycast in the direction he is looking
                {
                    if (attackCooldown <= 0)
                    {
                        chargingEffect.Play();
                        attacking = true;
                        originalPos = transform.position;
                    }
                }
            }

            if (attacking)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer<1)
                {
                    transform.position = new Vector3(transform.position.x + Random.Range(-0.025f, 0.025f), transform.position.y, transform.position.z + Random.Range(-0.025f, 0.025f));
                }

                if (attackTimer >= 1)
                {
                    if (attackTimer >= 2)
                        transform.Translate(Vector3.forward * moveSpeed * (5 - ((attackTimer - 2) * 10)) * Time.deltaTime); //(5 - ((attackTimer - 2) * 10))
                    else
                        transform.Translate(Vector3.forward * moveSpeed * 5 * Time.deltaTime); //5

                    chargeRotationTimer -= Time.deltaTime;
                    if (!charging)
                    {
                        attackingEffect.SetActive(true);
                        charging = true;

                        GameObject tempLookat = new GameObject(); //make the mulldozer charge at your direction after the windup, a bit finiky code bit
                        tempLookat.transform.position = Movement.playerPosition;
                        transform.LookAt(tempLookat.transform);

                        transform.Rotate(0, rotationDirection / 2, 0);
                        transform.position = originalPos;

                        Destroy(tempLookat);
                    }
                    if (chargeRotationTimer <= 0)
                    {
                        chargeRotationTimer = 0.15f; //0.25f
                        rotationDirection *= -1;
                        transform.Rotate(0, rotationDirection, 0);
                    }

                    if (attackTimer >= 2.5f)
                    {
                        StopAttacking();
                    }

                }
            }

        }
        else
        {
            dyingTimer -= Time.deltaTime;
            if (dyingTimer <= 0)
            {
                dying = false;
                Die();
            }
        }

        if (gotHit)
        {

            gotHit = false;
            gotHitTimer = 0.15f;
            Movement.swordHit = true;
            //hitEffect.Play();

            if (dying)
            {
                mulldozerMat.DOColor(new Color32(255, 0, 255, 255), 0.70f);
            }
            else
            {
                Sequence mulldozerHit = DOTween.Sequence();
                mulldozerHit.Append(mulldozerMat.DOColor(new Color32(255, 125, 255, 255), 0.25f));
                mulldozerHit.Append(mulldozerMat.DOColor(new Color32(255, 255, 255, 255), 0.25f));
            }
        }

        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;

            if (gotHitTimer <0)
            {
                rigid.velocity = Vector3.zero;
            }
            //transform.position = new Vector3(transform.position.x + (direction.x * Time.deltaTime * 5), transform.position.y, transform.position.z + (direction.y * Time.deltaTime * 5)); //originally *2 and not timedeltatime
        }

        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                stunned = false;
                stunnedEffect.SetActive(false);
            }
                

            transform.Rotate(0,720 * Time.deltaTime,0);
            
        }
    }

    public void StopAttacking()
    {
        attacking = false;
        attackingEffect.SetActive(false);
        charging = false;
        attackTimer = 0;
        chargeRotationTimer = 0.25f;
        attackCooldown = updatedAttackCooldown;
    }

    public void Die()
    {
        dyingEffect.transform.SetParent(null);
        dyingEffect.transform.rotation = Quaternion.identity;
        dyingEffect.transform.localScale = Vector3.one;
        dyingEffect.Play();
        Destroy(dyingEffect.gameObject, 3);
        Destroy(gameObject);
    }
}