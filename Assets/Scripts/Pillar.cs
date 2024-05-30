using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;

public class Pillar : MonoBehaviour
{
    //[SerializeField] Zone[] zones = new Zone[4];

    [SerializeField] Zone N;
    [SerializeField] Zone S;
    [SerializeField] Zone E;
    [SerializeField] Zone W;

    [SerializeField] float pushAmount;

    //[SerializeField] TextMeshProUGUI moveText;
    [SerializeField] GameObject moveText;

    float animationTime;

    bool busy;

    bool n;
    bool s;
    bool e;
    bool w;

    bool nR;
    bool sR;
    bool eR;
    bool wR;
    bool holdingPillar;

    Vector3 originalPos;
    Vector3 playerOriginalPos;

    void Start()
    {
        
    }


    void Update()
    {


        //Debug.Log(animationTime);
        if (busy)
        {
            animationTime += Time.deltaTime;

            Movement.Stun(0.5f);

            if (n)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x, originalPos.y, originalPos.z - (pushAmount/2)), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x, playerOriginalPos.y, playerOriginalPos.z - (pushAmount / 2)), animationTime*2);
                Movement.push = 1;
            }
            if (s)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x, originalPos.y, originalPos.z + (pushAmount/2)), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x, playerOriginalPos.y, playerOriginalPos.z + (pushAmount / 2)), animationTime*2);
                Movement.push = 1;
            }
            if (e)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x-(pushAmount/2), originalPos.y, originalPos.z), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x - (pushAmount / 2), playerOriginalPos.y, playerOriginalPos.z), animationTime*2);
                Movement.push = 1;
            }
            if (w)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x+(pushAmount/2), originalPos.y, originalPos.z), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x + (pushAmount / 2), playerOriginalPos.y, playerOriginalPos.z), animationTime*2);
                Movement.push = 1;
            }

            if (nR)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x, originalPos.y, originalPos.z - (pushAmount/2)), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x, playerOriginalPos.y, playerOriginalPos.z - (pushAmount / 2)), animationTime * 2);
                Movement.push = -1;
            }
            if (sR)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x, originalPos.y, originalPos.z + (pushAmount/2)), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x, playerOriginalPos.y, playerOriginalPos.z+(pushAmount/2)), animationTime*2);
                Movement.push = -1;
            }
            if (eR)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x - (pushAmount/2), originalPos.y, originalPos.z), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x - (pushAmount / 2), playerOriginalPos.y, playerOriginalPos.z), animationTime * 2);
                Movement.push = -1;
            }
            if (wR)
            {
                transform.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x + (pushAmount/2), originalPos.y, originalPos.z), animationTime*2);
                Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x + (pushAmount / 2), playerOriginalPos.y, playerOriginalPos.z), animationTime * 2);
                Movement.push = -1;
            }

            if (animationTime>=0.5)
            {
                //Movement.push = -2;

                animationTime = 0;

                busy = false;

                n = false;
                s = false;
                e = false;
                w = false;

                nR = false;
                sR = false;
                eR = false;
                wR = false;

            }

        }
        else
        {
            animationTime = 0;
        }




        if (N.inZone) //checking if I'm the pushing zone
        {
            if (Movement.playerYRotation >135 && Movement.playerYRotation <225) //checking if the player looking at the right direction
            {
                moveText.SetActive(true);
                if (!busy && Input.GetKey(KeyCode.Space)) //checking which direction the camera is facing :)
                {
                    Movement.busy = true; //preventing the player from moving while holding space
                    originalPos = transform.position;
                    Movement.playerPosition = N.transform.position; //snapping to the tile
                    Movement.playerYRotation = 180f;
                    Movement.UpdateYRotation();
                    playerOriginalPos = Movement.playerPosition;
                    if (!holdingPillar)
                    {
                        Movement.push = 0;
                        holdingPillar = true;
                    }
                    if ((Camera.main.transform.eulerAngles.y > 135 && Camera.main.transform.eulerAngles.y < 225))
                    {
                        if (Input.GetKey(KeyCode.W) && !S.immoveable)
                        {
                            busy = true;
                            n = true;
                        }
                        else if (Input.GetKey(KeyCode.S) && !Movement.cantPull)
                        {
                            busy = true;
                            sR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 315 || Camera.main.transform.eulerAngles.y < 45))
                    {
                        if (Input.GetKey(KeyCode.S) && !S.immoveable)
                        {
                            busy = true;
                            n = true;
                        }
                        else if (Input.GetKey(KeyCode.W) && !Movement.cantPull)
                        {
                            busy = true;
                            sR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 45 && Camera.main.transform.eulerAngles.y < 135))
                    {
                        if (Input.GetKey(KeyCode.D) && !S.immoveable)
                        {
                            busy = true;
                            n = true;
                        }
                        else if (Input.GetKey(KeyCode.A) && !Movement.cantPull)
                        {
                            busy = true;
                            sR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 225 && Camera.main.transform.eulerAngles.y < 315))
                    {
                        if (Input.GetKey(KeyCode.A) && !S.immoveable)
                        {
                            busy = true;
                            n = true;
                        }
                        else if (Input.GetKey(KeyCode.D) && !Movement.cantPull)
                        {
                            busy = true;
                            sR = true;
                        }

                    }

                }
                else if (!busy)
                {
                    Movement.push = -2;
                    Movement.busy = false;
                    holdingPillar = false;
                }


            }
            else
                moveText.SetActive(false);
        }
        if (S.inZone)
        {
            if (Movement.playerYRotation > 315 || Movement.playerYRotation < 45)
            {
                moveText.SetActive(true);
                if (!busy && Input.GetKey(KeyCode.Space)) 
                {
                    Movement.busy = true;
                    originalPos = transform.position;
                    Movement.playerPosition = S.transform.position;
                    Movement.playerYRotation = 0f;
                    Movement.UpdateYRotation();
                    playerOriginalPos = Movement.playerPosition;
                    if (!holdingPillar)
                    {
                        Movement.push = 0;
                        holdingPillar = true;
                    }
                    if ((Camera.main.transform.eulerAngles.y > 135 && Camera.main.transform.eulerAngles.y < 225))
                    {
                        if (Input.GetKey(KeyCode.S) && !N.immoveable)
                        {
                            busy = true;
                            s = true;
                        }
                        else if (Input.GetKey(KeyCode.W) && !Movement.cantPull)
                        {
                            busy = true;
                            nR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 315 || Camera.main.transform.eulerAngles.y < 45))
                    {
                        if (Input.GetKey(KeyCode.W) && !N.immoveable)
                        {
                            busy = true;
                            s = true;
                        }
                        else if (Input.GetKey(KeyCode.S) && !Movement.cantPull)
                        {
                            busy = true;
                            nR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 45 && Camera.main.transform.eulerAngles.y < 135))
                    {
                        if (Input.GetKey(KeyCode.A) && !N.immoveable)
                        {
                            busy = true;
                            s = true;
                        }
                        else if (Input.GetKey(KeyCode.D) && !Movement.cantPull)
                        {
                            busy = true;
                            nR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 225 && Camera.main.transform.eulerAngles.y < 315))
                    {
                        if (Input.GetKey(KeyCode.D) && !N.immoveable)
                        {
                            busy = true;
                            s = true;
                        }
                        else if (Input.GetKey(KeyCode.A) && !Movement.cantPull)
                        {
                            busy = true;
                            nR = true;
                        }

                    }

                }
                else if (!busy)
                {
                    Movement.push = -2;
                    Movement.busy = false;
                    holdingPillar = false;
                }

            }
            else
                moveText.SetActive(false);

        }
        if (E.inZone)
        {
            if (Movement.playerYRotation > 225 && Movement.playerYRotation < 315)
            {
                moveText.SetActive(true);
                if (!busy && Input.GetKey(KeyCode.Space)) 
                {
                    Movement.busy = true;
                    originalPos = transform.position;
                    Movement.playerPosition = E.transform.position; 
                    Movement.playerYRotation = 270f;
                    Movement.UpdateYRotation();
                    playerOriginalPos = Movement.playerPosition;
                    if (!holdingPillar)
                    {
                        Movement.push = 0;
                        holdingPillar = true;
                    }
                    if ((Camera.main.transform.eulerAngles.y > 135 && Camera.main.transform.eulerAngles.y < 225))
                    {
                        if (Input.GetKey(KeyCode.D) && !W.immoveable)
                        {
                            busy = true;
                            e = true;
                        }
                        else if (Input.GetKey(KeyCode.A) && !Movement.cantPull)
                        {
                            busy = true;
                            wR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 315 || Camera.main.transform.eulerAngles.y < 45))
                    {
                        if (Input.GetKey(KeyCode.A) && !W.immoveable)
                        {
                            busy = true;
                            e = true;
                        }
                        else if (Input.GetKey(KeyCode.D) && !Movement.cantPull)
                        {
                            busy = true;
                            wR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 45 && Camera.main.transform.eulerAngles.y < 135))
                    {
                        if (Input.GetKey(KeyCode.S) && !W.immoveable)
                        {
                            busy = true;
                            e = true;
                        }
                        else if (Input.GetKey(KeyCode.W) && !Movement.cantPull)
                        {
                            busy = true;
                            wR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 225 && Camera.main.transform.eulerAngles.y < 315))
                    {
                        if (Input.GetKey(KeyCode.W) && !W.immoveable)
                        {
                            busy = true;
                            e = true;
                        }
                        else if (Input.GetKey(KeyCode.S) && !Movement.cantPull)
                        {
                            busy = true;
                            wR = true;
                        }

                    }

                }
                else if (!busy)
                {
                    Movement.push = -2;
                    Movement.busy = false;
                    holdingPillar = false;
                }

            }
            else
                moveText.SetActive(false);
        }
        if (W.inZone)
        {
            if (Movement.playerYRotation > 45 && Movement.playerYRotation < 135)
            {
                moveText.SetActive(true);
                if (!busy && Input.GetKey(KeyCode.Space)) 
                {
                    Movement.busy = true;
                    originalPos = transform.position;
                    Movement.playerPosition = W.transform.position; 
                    Movement.playerYRotation = 90f;
                    Movement.UpdateYRotation();
                    playerOriginalPos = Movement.playerPosition;
                    if (!holdingPillar)
                    {
                        Movement.push = 0;
                        holdingPillar = true;
                    }
                    if ((Camera.main.transform.eulerAngles.y > 135 && Camera.main.transform.eulerAngles.y < 225))
                    {
                        if (Input.GetKey(KeyCode.A) && !E.immoveable)
                        {
                            busy = true;
                            w = true;
                        }
                        else if (Input.GetKey(KeyCode.D) && !Movement.cantPull)
                        {
                            busy = true;
                            eR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 315 || Camera.main.transform.eulerAngles.y < 45))
                    {
                        if (Input.GetKey(KeyCode.D) && !E.immoveable)
                        {
                            busy = true;
                            w = true;
                        }
                        else if (Input.GetKey(KeyCode.A) && !Movement.cantPull)
                        {
                            busy = true;
                            eR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 45 && Camera.main.transform.eulerAngles.y < 135))
                    {
                        if (Input.GetKey(KeyCode.W) && !E.immoveable)
                        {
                            busy = true;
                            w = true;
                        }
                        else if (Input.GetKey(KeyCode.S) && !Movement.cantPull)
                        {
                            busy = true;
                            eR = true;
                        }

                    }
                    if ((Camera.main.transform.eulerAngles.y > 225 && Camera.main.transform.eulerAngles.y < 315))
                    {
                        if (Input.GetKey(KeyCode.S) && !E.immoveable)
                        {
                            busy = true;
                            w = true;
                        }
                        else if (Input.GetKey(KeyCode.W) && !Movement.cantPull)
                        {
                            busy = true;
                            eR = true;
                        }

                    }

                }
                else if (!busy)
                {
                    Movement.push = -2;
                    Movement.busy = false;
                    holdingPillar = false;
                }

            }
            else
                moveText.SetActive(false);
        }
    }



    //timer = 0;
    //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);

    //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), animationTime);

}
