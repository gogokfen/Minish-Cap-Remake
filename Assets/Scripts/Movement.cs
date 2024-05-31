using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    float originalSpeed; // in case of running allowed
    bool rolling;
    float rollingSpeed;
    float rollingTimer;
    float rollingCooldown;

    [SerializeField] Zone backwardCheck;
    public static bool cantPull;

    //[SerializeField] Camera mainCamera;

    [SerializeField] Animator anim; 

    public static float hp;
    public static bool gotHit = false;
    public static int enemyHitAmount;
    float gotHitTimer;
    public static Vector2 enemyDirection;
    bool invul = false;
    float invulTimer;
    [SerializeField] Material linkMat; //for blinking effect

    public static bool mud = false;

    //[SerializeField] TextMeshPro linkHp;

    [SerializeField] GameObject sword;
    bool swordSwing = false;
    float swordTimer = 0;
    BoxCollider swordCol;

    [SerializeField] GameObject shield;
    public static bool shieldUp = false;
    public static bool enemyShielded = false;
    BoxCollider shieldCol;

    public static bool potUp = false;

    public static int push = -2; //idle

    //Transform originalTrans;
    Vector3 originalRot;

    //int directionX
    //Vector2 direction;

    public static float playerYRotation;
    static bool updateYRotation;

    public static Vector3 playerPosition;

    public static bool busy = false;

    public static bool midAction = false;

    static bool stunned;
    static float stunTime;
    float stunCount;

    float DirectionX;
    float DirectionY;

    void Start()
    {
        originalSpeed = moveSpeed;

        Cursor.lockState = CursorLockMode.Locked; //confined?
        hp = 12;

        linkMat.EnableKeyword(("_EMISSION"));

        swordCol = sword.GetComponent<BoxCollider>();
        shieldCol = shield.GetComponent<BoxCollider>();

        potUp = false;
    }

    void Update()
    {
        anim.SetInteger("Push", push);

        if (potUp)
        {
            if(stunned)
            {
                anim.Play("Pot Lift");
            }
        }


        if (busy || rolling)
        {
            sword.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            shield.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
        }


        if (backwardCheck.immoveable)
        {
            cantPull = true;
        }
        else
            cantPull = false;

        if (updateYRotation)
        {
            updateYRotation = false;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerYRotation, transform.eulerAngles.z);
        }
        else
            playerYRotation = transform.eulerAngles.y;

        hp = HealthSystem.currentHealth;

        if (gotHit)
        {
            gotHit = false;
            if (enemyShielded)
            {
                gotHitTimer = 0.15f;
            }
            if (!invul && !enemyShielded) //checking if the enemy actually got shielded or the shield was just up
            {
                gotHitTimer = 0.15f;
                invulTimer = 0.5f;
                invul = true;
                HealthSystem.TakeDamage(enemyHitAmount);
            }
            enemyShielded = false;
        }


        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x + (enemyDirection.x * Time.deltaTime *20), transform.position.y, transform.position.z + (enemyDirection.y * Time.deltaTime *20)); //originally *2 and not timedeltatime
        }


        if (invul)
        {
            invulTimer -= Time.deltaTime;

            //linkMat.SetColor("_BaseColor", Color.yellow);
            //linkMat.SetColor("_EmissionColor", Color.red * Mathf.Pow(2, 1*Mathf.Sin(invulTimer*50)));

            if (invulTimer<=0)
            {
                invul = false;
                //linkMat.SetColor("_EmissionColor", Color.black);
                //linkMat.SetColor("_BaseColor", Color.white);
            }
        }

        if (Input.GetKey(KeyCode.E) && busy)
        {
            //busy = true;
            //stunned = true;
            //Debug.Log(playerPosition);
            transform.position = playerPosition;
            //return; //stunned while holding space
        }
        /*
        else
            busy = false;
        */
        if (stunned)
        {
            stunCount += Time.deltaTime;
            transform.position = playerPosition;
            if (stunCount>stunTime)
            {
                stunned = false;
                stunCount = 0;
            }
            //return; //no bothering do other movement when stunned
        }
        else
        {
            playerPosition = transform.position;
        }



        //playerPosition = transform.position;

        //Debug.Log(playerYRotation);

        if (Input.GetMouseButtonDown(0) && !rolling && !busy && !potUp && gotHitTimer<0 && !shieldUp)
        {
            midAction = true;
            //Debug.Log("yes");
            swordCol.enabled = true;
            swordSwing = true;
            swordTimer = 0; 
            Stun(0.7f);
            sword.SetActive(true); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            shield.SetActive(true);
            //originalTrans.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            //originalRot = transform.eulerAngles; //???
            anim.Play("Attack",-1,0.15f);
            //anim.Play("Attack");

        }

        if (swordSwing)
        {
            swordTimer += (Time.deltaTime);
            //sword.transform.eulerAngles = Vector3.Lerp(new Vector3(originalTrans.eulerAngles.x, originalTrans.eulerAngles.y-90, originalTrans.eulerAngles.z), new Vector3(originalTrans.eulerAngles.x, originalTrans.eulerAngles.y+90, originalTrans.eulerAngles.z), swordTimer);
            //sword.transform.eulerAngles = Vector3.Lerp(new Vector3(originalRot.x, originalRot.y - 90, originalRot.z), new Vector3(originalRot.x, originalRot.y + 90, originalRot.z), swordTimer); // ORON PUT IN COMMENT WHEN ANIMATING

            if (swordTimer >= 0.7)
            {
                midAction = false;
                swordCol.enabled = false;
                swordSwing = false;
                swordTimer = 0;
                //sword.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            }
        }


        if (mud)
        {
            moveSpeed = originalSpeed * 0.6f;
        }
        else if (shieldUp)
        {
            moveSpeed = originalSpeed * 0.75f; //6
        }
        else
        {
            moveSpeed = originalSpeed;
        }

        if (Input.GetMouseButtonDown(1) && !rolling && !busy && !potUp) //shield
        {
            midAction = true;
            shield.SetActive(true); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            sword.SetActive(true);

            //Debug.Log("Shield up!");
            shieldCol.enabled = true;
            shieldUp = true;
            anim.Play("Block Start Anim");
            anim.SetBool("ShieldUp",true);
        }
        if (Input.GetMouseButtonUp(1) && !rolling && !busy && !potUp)
        {
            midAction = false;
            //shield.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            shieldCol.enabled = false;
            shieldUp = false;
            anim.SetBool("ShieldUp", false);

        }

        if (rolling)
        {
            rollingTimer -= Time.deltaTime; //0.75 seconds
            //6 *3 = 18 (0.75)
            //Debug.Log(rollingSpeed);
            rollingSpeed = (moveSpeed * 3 - (moveSpeed * ((1-rollingTimer)*2) ));
            transform.Translate(Vector3.forward * Time.deltaTime * rollingSpeed);

            shieldUp = false; //making sure shield isn't up when rolling
            anim.SetBool("ShieldUp", false);

            if (rollingTimer <= 0)
            {
                midAction = false;
                rolling = false;
                anim.SetBool("Rolling", false);
                rollingCooldown = 0.10f;
            }
                

        }
        else
        {
            rollingCooldown -= Time.deltaTime;
        }

        if (!stunned && !rolling && !busy)
        {

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S ) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //if any movement
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && rollingCooldown <= 0 && !potUp)
                {
                    midAction = true;
                    rolling = true;
                    anim.Play("Rolling");
                    anim.SetBool("Rolling", true);
                    rollingTimer = 0.75f; //0.3f //original was 0.25f
                }

                anim.SetBool("Moving", true);

                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

                if (Input.GetKey(KeyCode.W))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                        DirectionX = -0.7f;
                        DirectionY = 0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                        DirectionX = 0.7f;
                        DirectionY = 0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 0f, 0);
                        DirectionX = 0;
                        DirectionY = 1;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.W))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                        DirectionX = 0.7f;
                        DirectionY = 0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                        DirectionX = 0.7f;
                        DirectionY = -0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 90f, 0);
                        DirectionX = 1;
                        DirectionY = 0;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                        DirectionX = 0.7f;
                        DirectionY = -0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                        DirectionX = -0.7f;
                        DirectionY = -0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 180f, 0);
                        DirectionX = 0;
                        DirectionY = -1;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.S))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                        DirectionX = -0.7f;
                        DirectionY = -0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.W))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                        DirectionX = -0.7f;
                        DirectionY = 0.7f;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 270f, 0);
                        DirectionX = -1;
                        DirectionY = 0;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
            }
            else
            {
                anim.SetBool("Moving", false);
            }

        }
        else
            anim.SetBool("Moving", false);

    }

    public static void Stun(float newStunTime)
    {
        stunned = true;
        stunTime = newStunTime;
    }

    public static void SmallHit(Vector2 enemyDir)
    {
        enemyDirection = enemyDir;
        gotHit = true;
    }

    public static void UpdateYRotation()
    {
        updateYRotation = true;
    }
}

