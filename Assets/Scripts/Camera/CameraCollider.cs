using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Transform cameraOriginalPos;
    [SerializeField] float collisionDistance = 0.1f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float smoothSpeed = 5f;

    Vector3 desiredCameraPos;
    Vector3 direction;
    float distance;

    [SerializeField] Transform gustJarPov;
    //float lerpValue;

    void Update() // consider using late update
    {
        HandleCameraCollision();

        transform.position = Vector3.Lerp(transform.position, desiredCameraPos, smoothSpeed * Time.deltaTime);

    }

    void HandleCameraCollision()
    {
        direction = (transform.position - playerPos.position).normalized;
        distance = Vector3.Distance(playerPos.position, cameraOriginalPos.position);

        // ray cast from player to camera
        RaycastHit hit;
        if (Physics.Raycast(playerPos.position, direction, out hit, distance, obstacleLayer))
        {
            //move camera if object detected
            desiredCameraPos = hit.point - direction * collisionDistance;
        }
        else
        {
            // no object detected go back
            if (Movement.gustCamera)
            {
                desiredCameraPos = gustJarPov.position;
            }
            else
                desiredCameraPos = cameraOriginalPos.position;

        }
    }
}
