using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] Vector2 turn;
    [SerializeField] float sens = 0.5f;

    [SerializeField] Transform playerPos;

    [SerializeField] float min;
    [SerializeField] float max;
    void Start()
    {
        
    }

    void Update()
    {

        transform.position = playerPos.position;

        turn.x += Input.GetAxis("Mouse X") * sens;
        turn.y += Input.GetAxis("Mouse Y") * sens;


        if (turn.y<-40)
        {
            turn.y = -max;
        }
        if (turn.y > 15)
        {
            turn.y = -min;
        }
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
