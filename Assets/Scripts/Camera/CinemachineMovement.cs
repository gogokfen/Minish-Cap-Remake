using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineMovement : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    [SerializeField] float min;
    [SerializeField] float max;

    [SerializeField] GameObject gustJarVM;

    void Start()
    {
        
    }

    void Update()
    {
        if (Movement.gustCamera)
        {
            gustJarVM.SetActive(true);
        }
        else
        {
            if (gustJarVM.activeSelf)
            {
                gustJarVM.SetActive(false);
            }
        }


        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        if (transform.eulerAngles.x < max && transform.eulerAngles.x > min)
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0) * Time.deltaTime * rotateSpeed);
        }
    }
}
