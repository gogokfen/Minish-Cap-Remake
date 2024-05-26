using UnityEngine;

public class RotateOnTriggerEnter : MonoBehaviour
{
    public GameObject objectToRotate; // The object to rotate
    public float rotationSpeed = 10f; // The speed of rotation
    public Vector3 rotationAxis = Vector3.forward; // The axis of rotation
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
        // Check if the entering collider is tagged as the trigger object
        if (other.CompareTag("Player"))
        {
            CheckMovementKeys(); // Check for movement keys when player enters trigger
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the collider is tagged as the trigger object and the player is still inside
        if (other.CompareTag("Player"))
        {
            CheckMovementKeys(); // Check for movement keys while player stays inside trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rotateNow = false; // Player exits trigger, stop rotating
        }
    }

    private void CheckMovementKeys()
    {
        // Check if any movement keys are pressed
        if (Input.GetKey(keyToRotate))
        {
            rotateNow = true; // If any movement key is pressed, start rotating
        }
        else
        {
            rotateNow = false; // If no movement key is pressed, stop rotating
        }
    }
}
