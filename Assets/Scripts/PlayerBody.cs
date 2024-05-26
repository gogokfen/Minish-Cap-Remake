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

        if (!Movement.shieldUp)
            transform.rotation = Quaternion.Lerp(transform.rotation, Link.rotation, animTime); // divided by 2. build speed is different for some reason


        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
        {
            animTime = 0;
        }
        
    }
}
