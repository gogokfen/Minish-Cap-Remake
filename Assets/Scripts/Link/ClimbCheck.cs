using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClimbCheck : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] float maxHightDiff;

    //float smoothTime;
    [SerializeField]float smoothSpeed = 5;
    Vector3 newPos;
    //Vector3 playerOriginalPos;
    bool smooth = false;
    float groundY;
    void Start()
    {
        
    }

    void Update()
    {
        if (smooth)
        {
            //smoothTime += Time.deltaTime;
            //playerPos.position = Vector3.Lerp(playerPos.position, newPos, smoothTime * smoothSpeed);

            //playerPos.position = Vector3.Lerp(playerPos.position, new Vector3(playerPos.position.x,groundY,playerPos.position.z), smoothTime * smoothSpeed);
            //if (smoothTime >= 1/smoothSpeed) //smoothspeed = 1.5f
            //    smooth = false;

            playerPos.DOMove(new Vector3(playerPos.position.x, groundY, playerPos.position.z),smoothSpeed).SetEase(Ease.InCirc); //consider disabling gravity on trigger enter and reactivating on ontriggerexit
            smooth = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.position.y - playerPos.position.y);
        //Debug.Log("ground");
        if (other.transform.position.y > playerPos.position.y && (other.transform.position.y - playerPos.position.y< maxHightDiff))
        {
            //Movement.disableGravity = true;
            //Debug.Log("yes");
            smooth = true;
            //smoothTime = 0;
            //playerPos.position = new Vector3(playerPos.position.x, other.transform.position.y, playerPos.position.z);// too sharp need lerp
            //playerPos.position = new Vector3(playerPos.position.x, playerPos.position.y+((other.transform.position.y-playerPos.position.y)*2), playerPos.position.z);
            //newPos = new Vector3(playerPos.position.x, other.transform.position.y-0.5f, playerPos.position.z); //-0.375f
            //groundY = other.transform.position.y; //-0.5f
            groundY = playerPos.position.y + ((other.transform.position.y - playerPos.position.y) * 2);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Movement.disableGravity = false;
    }
}
