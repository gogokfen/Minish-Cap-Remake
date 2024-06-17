using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    Vector3 lastPos;
    float lastYRot;
    bool fall = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            /* version A
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            //tempDirection *= 1.5f;
            Movement.Stun(1);
            tempDirection.x = (tempDirection.x > 0) ? tempDirection.x + 1 : tempDirection.x - 1; //quicker than if statement
            tempDirection.z = (tempDirection.z > 0) ? tempDirection.z + 1 : tempDirection.z - 1;
            Movement.playerPosition = new Vector3(Movement.playerPosition.x + tempDirection.x, Movement.playerPosition.y+0.75f, Movement.playerPosition.z + tempDirection.z);
            */

            // Version B
            /*
            GameObject tempGO = new GameObject();
            tempGO.transform.position = new Vector3(Movement.playerPosition.x, Movement.playerPosition.y + 0.55f, Movement.playerPosition.z);
            //tempGO.transform.eulerAngles = new Vector3(0, -Movement.playerYRotation, 0);
            tempGO.transform.rotation = Quaternion.Euler(0, Movement.playerYRotation + 180, 0);
            tempGO.transform.Translate(Vector3.forward * 5);
            Movement.playerPosition = tempGO.transform.position;
            Movement.Stun(1);
            */
            //Version C check his last position before losing hight and put him there/in that direction

            //Version D make water collider higher and make a falling/drowning animation when he touches it before tping him back
            lastPos = Movement.playerPosition;
            lastPos.y = transform.position.y; //making sure he is begova ha maim
            lastYRot = Movement.playerYRotation;
            fall = true;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (fall)
            {
                fall = false;
                GameObject tempGO = new GameObject();
                tempGO.transform.position = lastPos;
                tempGO.transform.rotation = Quaternion.Euler(0, lastYRot + 180, 0);
                tempGO.transform.Translate(Vector3.forward * 2);
                Movement.playerPosition = tempGO.transform.position;
                Movement.Stun(0.25f);
            }
        }
    }
}

/**
 * 
 *     private void OnTriggerStay(Collider other)
    {
        
        
        if (other.tag == "Player")
        {
            if (Movement.playerPosition.y<=transform.position.y && !fall)
            {
                fall = true;
                lastPos = Movement.playerPosition;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (fall)
            {
                Movement.playerPosition = lastPos;
                Movement.Stun(0.5f);
            }
        }
    }
 * 
 */
