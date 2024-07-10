using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineMovement : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    [SerializeField] float min;
    [SerializeField] float max;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        if (transform.eulerAngles.x < max && transform.eulerAngles.x > min)
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0) * Time.deltaTime * rotateSpeed);
        }
    }
}
