using UnityEngine;

public class RotateOnTriggerEnter : MonoBehaviour
{
    public GameObject objectToRotate; 
    public float rotationSpeed = 10f; 
    public Vector3 rotationAxis = Vector3.forward; 
    public KeyCode keyToRotate;
    private bool rotateNow;

    private void Update()
    {
        if (rotateNow)
        {
            objectToRotate.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            CheckMovementKeys(); 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            CheckMovementKeys(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rotateNow = false; 
        }
    }

    private void CheckMovementKeys()
    {
        
        if (Input.GetKey(keyToRotate))
        {
            rotateNow = true; 
        }
        else
        {
            rotateNow = false; 
        }
    }
}
