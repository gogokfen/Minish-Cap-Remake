using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField] Transform Link;

    float animTime;

    void Update()
    {
        transform.position = Link.position;

        animTime += Time.deltaTime;

        if (!Movement.shieldUp || !DebugMode.mobileShield) //puting an ease between the player movement and the visual body movement, but not while shield is up
            transform.rotation = Quaternion.Lerp(transform.rotation, Link.rotation, animTime); // divided by 2. build speed is different for some reason

        if (Movement.gustCamera)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
        }
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
        {
            animTime = 0;
        }
    }
}
