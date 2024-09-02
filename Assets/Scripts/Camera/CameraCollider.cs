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
    [SerializeField] Transform gustJarPov;
    Vector3 desiredCameraPos;
    Vector3 direction;
    float distance;

    void Update() // consider using late update
    {
        HandleCameraCollision();

        transform.position = Vector3.Lerp(transform.position, desiredCameraPos, smoothSpeed * Time.deltaTime);
    }

    void HandleCameraCollision()
    {
        direction = (transform.position - playerPos.position).normalized;
        distance = Vector3.Distance(playerPos.position, cameraOriginalPos.position);

        RaycastHit hit;
        if (Physics.Raycast(playerPos.position, direction, out hit, distance, obstacleLayer)) //bringing the camera closer to the player if raycast hits obstacle
        {
            desiredCameraPos = hit.point - direction * collisionDistance;
        }
        else
        {
            if (Movement.gustCamera) //gust jar has it's own camera position
            {
                desiredCameraPos = gustJarPov.position;
            }
            else
                desiredCameraPos = cameraOriginalPos.position;
        }
    }
}
