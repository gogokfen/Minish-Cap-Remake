using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuChu : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public int hp;
    float moveTimer;
    float moveAnim;
    bool move = false;
    Vector3 direction;

    float jumpTimer;
    [SerializeField] float jumpCooldown = 6;
    bool jumping = false;
    bool located = false;
    float jumpAnim;
    Vector3 playerLastPos;
    Vector3 halfWayPoint;
    Vector3 chuchuLastPos;

    [HideInInspector]
    public bool vulnerable = false;
    [HideInInspector]
    public bool fallen = false;
    float waddleTime;
    int fallDirection = 0; //consider deleteing later
    [HideInInspector]
    public bool wakingUp = false;

    float attackTimer;
    [SerializeField] float attackCooldown = 5;
    bool attacking = false;
    int attackRoll;
    bool spat = false;
    [SerializeField] GameObject targetedSpit;
    [SerializeField] GameObject shakeSpit;
    [SerializeField] Transform shakeSpot;
    float shakeTimer;
    Vector3 shakeDir;


    //GameObject tempBody;
    [SerializeField] Transform link;
    [SerializeField] GameObject GFX;


    void Start()
    {

        //tempBody = new GameObject();
    }

    void Update()
    {
        moveTimer += Time.deltaTime;

        if (!vulnerable && !attacking)
        {
            GFX.transform.LookAt(link);
            GFX.transform.eulerAngles = new Vector3(0, GFX.transform.eulerAngles.y, 0);
        }

        if (!vulnerable)
        {
            if (!attacking)
            {
                jumpTimer += Time.deltaTime;
            }
            if (!jumping)
            {
                attackTimer += Time.deltaTime;
            }
        }

        if (moveTimer >= 1 && !vulnerable && !jumping && !attacking)
        {
            moveTimer = 0;
            direction = (Movement.playerPosition - transform.position);
            direction.y = 0;

            //tempBody.transform.position = Movement.playerPosition;
            //GFX.transform.LookAt(link);
            //GFX.transform.eulerAngles = new Vector3(0, GFX.transform.eulerAngles.y, 0);

            move = true;
        }
        if (move)
        {
            moveAnim += Time.deltaTime;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            if (moveAnim >= 0.5)
            {
                move = false;
                moveAnim = 0;
            }


        }


        if (attackTimer >= attackCooldown)
        {
            if (!attacking)
            {
                attackRoll = Random.Range(0, 2);
            }
            attacking = true;

            if (attackRoll == 1)
            {
                if (attackTimer >= attackCooldown + 2)
                {
                    GFX.transform.LookAt(link);
                    GFX.transform.eulerAngles = new Vector3(0, GFX.transform.eulerAngles.y, 0);
                    spat = false;
                    attacking = false;
                    attackTimer = 0;
                }
                else if (attackTimer >= attackCooldown + 1)
                {
                    //SFXController.PlaySFX("ChuChuSpit", 1); add sound
                    if (!spat)
                    {
                        GameObject tempSpit = Instantiate(targetedSpit, new Vector3(transform.position.x, 4, transform.position.z), transform.rotation);
                        tempSpit.GetComponent<ChuChuTargetedSpit>().spitTarget = link;
                    }
                    spat = true;
                }
                else if (attackTimer >= attackCooldown + 0.75f)
                {
                    GFX.transform.Rotate(225 * Time.deltaTime, 0, 0);
                }
                else if (attackTimer >= attackCooldown)
                {
                    GFX.transform.Rotate(-45 * Time.deltaTime, 0, 0);
                }
            }

            else if (attackRoll == 0)
            {
                if (attackTimer >= attackCooldown + 2)
                {
                    attacking = false;
                    attackTimer = 0;
                }
                if (shakeTimer>0)
                {
                    shakeTimer -= Time.deltaTime;
                    GFX.transform.Rotate(shakeDir*4*Time.deltaTime);
                }
                else
                {
                    shakeDir = new Vector3(Random.Range(-45, 15), Random.Range(0, 360), 0);
                    GameObject tempShake = Instantiate(shakeSpit, shakeSpot.position,Quaternion.Euler(shakeDir)); //Quaternion.Euler(shakeSpot.eulerAngles+shakeSpit.transform.eulerAngles)
                    shakeTimer = 0.05f;
                    GFX.transform.rotation = Quaternion.identity;
                    shakeDir = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
                }
                
                

            }


        }

        if (jumpTimer >= jumpCooldown) //10
        {
            if (!jumping && !attacking)
            {
                jumping = true;
            }

            if (jumpTimer > jumpCooldown + 1) //11
            {
                if (!located)
                {
                    located = true;
                    playerLastPos = Movement.playerPosition;
                    chuchuLastPos = transform.position;
                    halfWayPoint = new Vector3((playerLastPos.x + chuchuLastPos.x) / 2, 6, (playerLastPos.z + chuchuLastPos.z) / 2);
                }
                jumpAnim += Time.deltaTime; // /1f
                if (jumpTimer > jumpCooldown + 1.5)
                    transform.position = Vector3.Lerp(halfWayPoint, playerLastPos, (jumpAnim * 2) - 1);
                else
                    transform.position = Vector3.Lerp(chuchuLastPos, halfWayPoint, jumpAnim * 2);
            }

            if (jumpTimer >= jumpCooldown + 2) //14
            {
                jumpAnim = 0;
                jumpTimer = 0;
                jumping = false;
                located = false;
            }
        }

        if (vulnerable)
        {
            waddleTime += Time.deltaTime;


            if (waddleTime >= 5)
            {
                if (fallDirection == 0)
                {
                    fallDirection = Random.Range(1, 3);
                    if (fallDirection == 2)
                    {
                        fallDirection = -1;
                    }
                }

                if (waddleTime >= 6 && waddleTime <= 7)
                {
                    fallen = true;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90 * fallDirection * Time.deltaTime);
                }
                else if (waddleTime >= 11)
                {
                    wakingUp = true;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90 * -fallDirection * Time.deltaTime);
                }

                if (waddleTime >= 12)
                {
                    transform.eulerAngles = Vector3.zero;
                    fallDirection = 0;
                    vulnerable = false;
                    wakingUp = false;
                    fallen = false;
                    waddleTime = 0;
                }

            }
            else
            {
                direction = (Movement.playerPosition - transform.position);
                transform.Translate(new Vector3(0, 0, direction.z) * moveSpeed * Time.deltaTime);
            }
        }

    }
}
