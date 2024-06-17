using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuChu : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public int hp;
    float moveTimer;
    float moveAnim;
    float jumpTimer;
    bool jumping = false;
    float jumpAnim;
    Vector3 playerLastPos;
    Vector3 halfWayPoint;
    Vector3 chuchuLastPos;
    bool move = false;
    [HideInInspector]
    public bool vulnerable = false;
    float waddleTime;
    int fallDirection = 0; //consider deleteing later
    [HideInInspector]
    public bool wakingUp = false;

    Vector3 direction;

    GameObject tempBody;
    [SerializeField] GameObject GFX;
    void Start()
    {
        tempBody = new GameObject();
    }

    void Update()
    {
        moveTimer += Time.deltaTime;
        if (!vulnerable)
            jumpTimer += Time.deltaTime;
        if (moveTimer >= 1 && !vulnerable && !jumping)
        {
            moveTimer = 0;
            direction = (Movement.playerPosition - transform.position);
            direction.y = 0;

            tempBody.transform.position = Movement.playerPosition;
            GFX.transform.LookAt(tempBody.transform);

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

        if (jumpTimer>=6) //10
        {
            if (!jumping)
            {
                jumping = true;
                playerLastPos = Movement.playerPosition;
                chuchuLastPos = transform.position;
                halfWayPoint = new Vector3((playerLastPos.x + chuchuLastPos.x) / 2, 6, (playerLastPos.z + chuchuLastPos.z) / 2);
            }

            if (jumpTimer>7) //11
            {
                jumpAnim += Time.deltaTime; // /1f
                if (jumpTimer > 7.5)
                    transform.position = Vector3.Lerp(halfWayPoint, playerLastPos, (jumpAnim * 2)-1);
                else
                    transform.position = Vector3.Lerp(chuchuLastPos, halfWayPoint, jumpAnim * 2);
                //transform.position = Vector3.Lerp(chuchuLastPos, playerLastPos, jumpAnim);
            }
            
            if (jumpTimer>=8) //14
            {
                jumpAnim = 0;
                jumpTimer = 0;
                jumping = false;
            }
        }

        if (vulnerable)
        {
            waddleTime += Time.deltaTime;


            if (waddleTime>=5)
            {
                if (fallDirection==0)
                {
                    fallDirection = Random.Range(1, 3);
                    if (fallDirection == 2)
                    {
                        fallDirection = -1;
                    }
                }
                    
                if (waddleTime>=6 && waddleTime<=7)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90 * fallDirection * Time.deltaTime);
                }
                else if (waddleTime>=11)
                {
                    wakingUp = true;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90 * -fallDirection * Time.deltaTime);
                }

                if (waddleTime>=12)
                {
                    transform.eulerAngles = Vector3.zero;
                    fallDirection = 0;
                    vulnerable = false;
                    wakingUp = false;
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
