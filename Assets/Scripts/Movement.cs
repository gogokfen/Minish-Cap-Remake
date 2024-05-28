using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    float originalSpeed; // in case of running allowed
    bool rolling;
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

    [SerializeField] GameObject shield;
    public static bool shieldUp;
    public static bool enemyShielded;

    public static bool potUp;

    //Transform originalTrans;
    Vector3 originalRot;

    //int directionX
    //Vector2 direction;

    public static float playerYRotation;
    static bool updateYRotation;

    public static Vector3 playerPosition;

    public static bool busy = false;

    static bool stunned;
    static float stunTime;
    float stunCount;



    void Start()
    {
        originalSpeed = moveSpeed;

        Cursor.lockState = CursorLockMode.Locked; //confined?
        hp = 12;

        linkMat.EnableKeyword(("_EMISSION"));
    }

    void Update()
    {
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
        //linkHp.text = "Hp: " + hp;

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

        if (Input.GetKey(KeyCode.Space) && busy)
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

        if (Input.GetMouseButtonDown(0) && !rolling && !busy && !potUp && gotHitTimer<0)
        {
            swordSwing = true;
            swordTimer = 0;
            Stun(0.2f);
            sword.SetActive(true);
            //originalTrans.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            originalRot = transform.eulerAngles;
        }

        if (swordSwing)
        {
            swordTimer += (Time.deltaTime * 5);
            //sword.transform.eulerAngles = Vector3.Lerp(new Vector3(originalTrans.eulerAngles.x, originalTrans.eulerAngles.y-90, originalTrans.eulerAngles.z), new Vector3(originalTrans.eulerAngles.x, originalTrans.eulerAngles.y+90, originalTrans.eulerAngles.z), swordTimer);
            sword.transform.eulerAngles = Vector3.Lerp(new Vector3(originalRot.x, originalRot.y - 90, originalRot.z), new Vector3(originalRot.x, originalRot.y + 90, originalRot.z), swordTimer);

            if (swordTimer >= 1)
            {
                swordSwing = false;
                swordTimer = 0;
                sword.SetActive(false);
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

        if (Input.GetMouseButton(1) && !rolling && !busy && !potUp) //shield
        {
            shield.SetActive(true);
            shieldUp = true;
        }
        else
        {
            shield.SetActive(false);
            shieldUp = false;
        }

        if (rolling)
        {
            rollingTimer -= Time.deltaTime;
            transform.Translate(Vector3.forward * Time.deltaTime * (moveSpeed * 2.0f)); 

            if (rollingTimer <= 0)
            {
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
                    rolling = true;
                    anim.SetBool("Rolling", true);
                    rollingTimer = 0.75f; //0.3f //original was 0.25f
                }

                anim.SetBool("Moving", true);

                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

                if (Input.GetKey(KeyCode.W))
                {
                    if (Input.GetKey(KeyCode.A))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                    else if (Input.GetKey(KeyCode.D))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                    else
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 0f, 0);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    if (Input.GetKey(KeyCode.W))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                    else if (Input.GetKey(KeyCode.S))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                    else
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 90f, 0);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    if (Input.GetKey(KeyCode.D))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                    else if (Input.GetKey(KeyCode.A))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                    else
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 180f, 0);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (Input.GetKey(KeyCode.S))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                    else if (Input.GetKey(KeyCode.W))
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                    else
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 270f, 0);
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

//Debug.Log(transform.eulerAngles.y);
/*
Debug.Log(mainCamera.transform.eulerAngles.y + 90f);
if (transform.eulerAngles.y < mainCamera.transform.eulerAngles.y + 90f)
{
    transform.eulerAngles = new Vector3(0, transform.rotation.y + 0.1f,0);
}
*/

//transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 500);
/*
if (transform.rotation.eulerAngles.y < mainCamera.transform.eulerAngles.y)
{
    transform.Rotate(new Vector3(0, 1f, 0));
}
*/

//transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 500);

/*
this.transform.rotation = Camera.main.transform.rotation;
Vector3 localHeadRotation = this.transform.localRotation.eulerAngles;
localHeadRotation.y = Mathf.Clamp(localHeadRotation.y, 0, 360);
this.transform.localRotation = Quaternion.Euler(localHeadRotation);

*/


//transform.Translate(transform. * Time.deltaTime * moveSpeed,Space.Self);