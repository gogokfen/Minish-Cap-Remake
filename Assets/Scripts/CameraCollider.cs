using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Transform CameraOriginalPos;

    bool collided = false;
    float collisionTimeout = 0; //0.5f;
    float maxTimeout = 0.125f;

    [SerializeField] float pushAmount;

    float accelAmount;

    Vector3 direction;
    Vector3 revDirection;

    //string lastCollision;
    int lastCollision;
    void Start()
    {

    }


    void Update()
    {
        if (!collided)
        {
            collisionTimeout += Time.deltaTime;

            if (collisionTimeout > maxTimeout) //0.125f
            {
                revDirection = (CameraOriginalPos.position - transform.position);
                transform.Translate(revDirection * Time.deltaTime * pushAmount * 2, Space.World);
            }

        }
        else
        {
            Vector3 direction = (playerPos.position - transform.position);

            accelAmount = Vector3.Distance(transform.position, playerPos.position);
            accelAmount /= 2f;
            accelAmount = Mathf.Pow(accelAmount, 2); //making it non linear
            transform.Translate(direction * Time.deltaTime * accelAmount, Space.World);
        }


        //Vector3 tempDirection = (playerPos.position - transform.position);

        //transform.Translate(tempDirection * Time.deltaTime * pushAmount,Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Moveable")
        {
            if (other.GetHashCode() == lastCollision)
            {
                maxTimeout *= 1.5f;

                if (maxTimeout > 2f)
                    maxTimeout = 2f;
            }
            else
                maxTimeout = 0.125f;
        }

        //Debug.Log("enter");
        /*
        if (other.tag == "moveable")
        {
            Vector3 tempDirection = (other.transform.position - transform.position);
            transform.Translate(tempDirection * Time.deltaTime * 100);


        }
        */
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Moveable") //&& other.GetHashCode() !=lastCollision
        {
            collisionTimeout = 0;
            collided = true;

            /*
            if (other.GetHashCode() == lastCollision)
            {
                Debug.Log(maxTimeout);
                maxTimeout *= 1.5f;
            }
            else
                maxTimeout = 0.125f;
            */

            //Debug.Log("stay");
            //Vector3 tempDirection = (other.transform.position - transform.position);
            //Vector3 tempDirection = (transform.position - other.transform.position );

            //Debug.Log(tempDirection);
            /*
            Vector3 tempDirection = (playerPos.position - transform.position);

            //transform.Translate(tempDirection * Time.deltaTime * pushAmount,Space.World);

            accelAmount = Vector3.Distance(transform.position, playerPos.position);
            accelAmount /= 2f;
            accelAmount = Mathf.Pow(accelAmount, 2); //making it non linear
            transform.Translate(tempDirection * Time.deltaTime * accelAmount, Space.World);
            */

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Moveable")
        {

            lastCollision = other.GetHashCode();
            collided = false;
            //lastCollision = other.name;
        }


        /*
        if (other.tag == "Moveable")
        {
            Vector3 tempRevDirection = (CameraOriginalPos.position - transform.position);
            transform.Translate(tempRevDirection * Time.deltaTime * pushAmount, Space.World);
            //transform.position = CameraOriginalPos.position;
        }
        */
    }
}
