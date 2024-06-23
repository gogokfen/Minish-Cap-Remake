using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuChuLegs : MonoBehaviour
{
    [SerializeField] ChuChu chuchu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chuchu.wakingUp)
        {
            if (transform.localScale.x<1)
            {
                //transform.localScale *= 1.0015f;
                transform.localScale *= 1+(Time.deltaTime/1.25f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar")
        {
            if (transform.localScale.magnitude > 0.80f) //0.85f
            {
                //transform.localScale /= 1.004f;
                transform.localScale /= 1 + (Time.deltaTime/6f);
            }
            else
            {
                chuchu.vulnerable = true;
            }
            
            
        }
    }
}
