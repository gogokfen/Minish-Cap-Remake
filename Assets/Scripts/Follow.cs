using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.position;
    }
}
