using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Mushroom : MonoBehaviour
{
    [SerializeField] Zone N;
    [SerializeField] Zone S;
    [SerializeField] Zone E;
    [SerializeField] Zone W;

    [SerializeField] TextMeshProUGUI windUpText;

    [SerializeField] int maximumWindup;

    float animationTime;
    float windUp = 0;

    bool busy;

    bool n;
    bool s;
    bool e;
    bool w;

    Vector3 playerOriginalPos;

    Vector3 halfWayPoint;

    void Update()
    {
        if (windUp > 0f && !(Input.GetKey(KeyCode.Space) || Input.GetMouseButton(2)))
        {
            ActionText.UpdateText("");
            if (windUp>=1)
            {
                animationTime += Time.deltaTime;
                busy = true;
                Movement.Stun(1);

                if (windUp>maximumWindup)
                {
                    windUp = maximumWindup;
                }
                // deciding in which way to launch to player depending on his location
                if (n)
                {
                    Movement.disableGravity = true;

                    halfWayPoint = new Vector3(playerOriginalPos.x,playerOriginalPos.y + (2 + (int)windUp)*1.5f, playerOriginalPos.z + ((2 + (int)windUp)/2));
                    
                    if (animationTime<=0.5f)
                        Movement.playerPosition = Vector3.Lerp(playerOriginalPos, halfWayPoint ,(animationTime*2));
                    else
                        Movement.playerPosition = Vector3.Lerp(halfWayPoint, new Vector3(Movement.playerPosition.x, playerOriginalPos.y, Movement.playerPosition.z), ( (animationTime * 2)-1));
                    Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x, Movement.playerPosition.y, playerOriginalPos.z + (2 + (int)windUp)), animationTime);
                }
                if (s)
                {
                    Movement.disableGravity = true;

                    halfWayPoint = new Vector3(playerOriginalPos.x, playerOriginalPos.y + (2 + (int)windUp)*1.5f, playerOriginalPos.z - ((2 + (int)windUp) / 2));

                    if (animationTime <= 0.5f)
                        Movement.playerPosition = Vector3.Lerp(playerOriginalPos, halfWayPoint, (animationTime * 2));
                    else
                        Movement.playerPosition = Vector3.Lerp(halfWayPoint, new Vector3(Movement.playerPosition.x, playerOriginalPos.y, Movement.playerPosition.z), ((animationTime * 2) - 1));
                    Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x, Movement.playerPosition.y, playerOriginalPos.z - (2 + (int)windUp)), animationTime);
                }
                if (e)
                {
                    Movement.disableGravity = true;

                    halfWayPoint = new Vector3(playerOriginalPos.x + ((2 + (int)windUp) / 2), playerOriginalPos.y + (2 + (int)windUp)*1.5f, playerOriginalPos.z);

                    if (animationTime <= 0.5f)
                        Movement.playerPosition = Vector3.Lerp(playerOriginalPos, halfWayPoint, (animationTime * 2));
                    else
                        Movement.playerPosition = Vector3.Lerp(halfWayPoint, new Vector3(Movement.playerPosition.x, playerOriginalPos.y, Movement.playerPosition.z), ((animationTime * 2) - 1));
                    Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x + (2 + (int)windUp), Movement.playerPosition.y, playerOriginalPos.z), animationTime);
                }
                if (w)
                {
                    Movement.disableGravity = true;

                    halfWayPoint = new Vector3(playerOriginalPos.x - ((2 + (int)windUp) / 2), playerOriginalPos.y + (2 + (int)windUp)*1.5f, playerOriginalPos.z);

                    if (animationTime <= 0.5f)
                        Movement.playerPosition = Vector3.Lerp(playerOriginalPos, halfWayPoint, (animationTime * 2));
                    else
                        Movement.playerPosition = Vector3.Lerp(halfWayPoint, new Vector3(Movement.playerPosition.x, playerOriginalPos.y, Movement.playerPosition.z), ((animationTime * 2) - 1));
                    Movement.playerPosition = Vector3.Lerp(playerOriginalPos, new Vector3(playerOriginalPos.x - (2 + (int)windUp), Movement.playerPosition.y, playerOriginalPos.z), animationTime);
                }
            }
            else
            {
                windUp = 0;
            }


            if (animationTime >= 1) //2.5f
            {
                animationTime = 0;
                windUp = 0;

                busy = false;

                n = false;
                s = false;
                e = false;
                w = false;

                Movement.disableGravity = false;
            }
        }
        else
        {
            animationTime = 0;
            windUpText.text = "";
        }
            

        if (!Movement.midAction)
        {
            if (N.inZone) 
            {
                if (Movement.playerYRotation > 135 && Movement.playerYRotation < 225) //checking if the player looking at the right direction
                {
                    ActionText.UpdateText("Grab");
                    if (!busy && Input.GetKey(KeyCode.Space)) //checking which direction the camera is facing :)
                    {
                        Movement.busy = true;
                        playerOriginalPos = Movement.playerPosition;
                        windUp += (Time.deltaTime * 2);
                        windUpText.text = "" + (int)windUp;
                        s = true;
                    }
                    else
                        Movement.busy = false;
                }
                else
                    ActionText.UpdateText("");

            }
            if (S.inZone)
            {
                if (Movement.playerYRotation > 315 || Movement.playerYRotation < 45)
                {
                    ActionText.UpdateText("Grab");
                    if (!busy && Input.GetKey(KeyCode.Space))
                    {
                        Movement.busy = true;
                        playerOriginalPos = Movement.playerPosition;
                        windUp += (Time.deltaTime * 2);
                        windUpText.text ="" + (int)windUp;
                        n = true;
                    }
                    else
                        Movement.busy = false;
                }
                else
                    ActionText.UpdateText("");
            }
            if (E.inZone)
            {
                if (Movement.playerYRotation > 225 && Movement.playerYRotation < 315)
                {
                    ActionText.UpdateText("Grab");
                    if (!busy && Input.GetKey(KeyCode.Space))
                    {
                        Movement.busy = true;
                        playerOriginalPos = Movement.playerPosition;
                        windUp += (Time.deltaTime *2);
                        windUpText.text = "" + (int)windUp;
                        //busy = true;
                        w = true;
                    }
                    else
                        Movement.busy = false;
                }
                else
                    ActionText.UpdateText("");
            }
            if (W.inZone)
            {
                if (Movement.playerYRotation > 45 && Movement.playerYRotation < 135)
                {
                    ActionText.UpdateText("Grab");
                    if (!busy && Input.GetKey(KeyCode.Space))
                    {
                        Movement.busy = true;
                        playerOriginalPos = Movement.playerPosition;
                        windUp += (Time.deltaTime * 2);
                        windUpText.text = "" + (int)windUp;
                        e = true;
                    }
                    else
                        Movement.busy = false;
                }
                else
                    ActionText.UpdateText("");
            }
        }
    }
}
