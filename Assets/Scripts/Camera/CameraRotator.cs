using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] Vector2 turn;
    public float sens = 0.5f;

    [SerializeField] Transform playerPos;

    [SerializeField] float min;
    [SerializeField] float max;

    [SerializeField] float yEasing;

    void Update()
    {
        if (Movement.cutScene) // Sometimes we don't want camera movement
        {
            return;
        }
        if (PauseMenu.paused)
        {
            return;
        }

        transform.position = new Vector3(playerPos.position.x, transform.position.y + ((playerPos.position.y - transform.position.y)*yEasing), playerPos.position.z); //making hight changes less jarring
        
        turn.x += Input.GetAxis("Mouse X") * sens;
        turn.y += Input.GetAxis("Mouse Y") * sens;


        if (turn.y<-40) //limiting the camera movement
        {
            turn.y = -max;
        }
        if (turn.y > 15)
        {
            turn.y = -min;
        }
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0); //preventing rotation on the Z axis

        if (HealthSystem.currentHealth<=0)
        {
            Destroy(this);
        }
    }
}
