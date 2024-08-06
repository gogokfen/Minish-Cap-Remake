using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField] Transform Link;

    float animTime;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Link.position;

        //transform.rotation = Link.rotation;

        animTime += Time.deltaTime;

        if (!Movement.shieldUp || DebugMode.mobileShield)
            transform.rotation = Quaternion.Lerp(transform.rotation, Link.rotation, animTime); // divided by 2. build speed is different for some reason

        
        if (Movement.gustCamera) //Movement.gustJarUp
        {
            //transform.rotation = Camera.main.transform.rotation;
            //transform.rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, transform.rotation.z, transform.rotation.w);

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
        }
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
        {
            animTime = 0;
        }
        
    }
}
