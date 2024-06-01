using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
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
            GameObject tempGO = new GameObject();
            tempGO.transform.position = new Vector3(Movement.playerPosition.x, Movement.playerPosition.y + 0.55f, Movement.playerPosition.z);
            //tempGO.transform.eulerAngles = new Vector3(0, -Movement.playerYRotation, 0);
            tempGO.transform.rotation = Quaternion.Euler(0, Movement.playerYRotation + 180, 0);
            tempGO.transform.Translate(Vector3.forward * 5);
            Movement.playerPosition = tempGO.transform.position;
            Movement.Stun(1);
        }
    }
}
