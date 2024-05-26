using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    private Camera cameraToLookAt;

    private void Start() 
    {
        // Assuming you want to use the main camera
        cameraToLookAt = Camera.main;

        // Uncomment the following line if you specifically need to find any camera (not just the main camera)
        // cameraToLookAt = FindAnyObjectByType<Camera>();
    }

    private void Update() 
    {
        if (cameraToLookAt != null)
        {
            // Make the object face the camera directly
            Vector3 direction = cameraToLookAt.transform.position - transform.position;
            direction.y = 0; // Keep the object upright by nullifying the y component
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
