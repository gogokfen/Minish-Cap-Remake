using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeGravity : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        Movement.disableGravity = true;
    }


    private void OnTriggerExit(Collider other) 
    {
        Movement.disableGravity = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
