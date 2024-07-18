using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mulldozer : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public int hp;
    float moveTimer;

    bool attacking;
    bool charging;
    float attackCooldown;
    float attackTimer;
    float rotationDirection = 90;
    float chargeRotationTimer = 0.25f;

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

    //public ParticleSystem chargingEffect;

    //public ParticleSystem attackingEffect;


    void Start()
    {
        
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
                    transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
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
                if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out player, Quaternion.identity, 10, mask)) //raycast in the direction he is looking
                {
                    if (attackCooldown <= 0)
                    {
                        attacking = true;
                    }
                }
            }

            if (attacking)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= 1)
                {
                    if (attackTimer >= 2)
                        transform.Translate(Vector3.forward * moveSpeed * (5 - ((attackTimer - 2) * 10)) * Time.deltaTime);
                    else
                        transform.Translate(Vector3.forward * moveSpeed * 5 * Time.deltaTime);

                    chargeRotationTimer -= Time.deltaTime;
                    if (!charging)
                    {
                        charging = true;
                        transform.Rotate(0, rotationDirection / 2, 0);
                    }
                    if (chargeRotationTimer <= 0)
                    {
                        chargeRotationTimer = 0.25f;
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
        }

        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x + (direction.x * Time.deltaTime * 5), transform.position.y, transform.position.z + (direction.y * Time.deltaTime * 5)); //originally *2 and not timedeltatime
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
        charging = false;
        attackTimer = 0;
        chargeRotationTimer = 0.25f;
        attackCooldown = 3;
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
