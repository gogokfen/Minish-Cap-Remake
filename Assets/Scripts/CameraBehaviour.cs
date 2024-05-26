using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] float rotationSpeed; //1500. build speed is different for some reason
    [SerializeField] Transform player;
    [SerializeField] Transform cameraPos;

    Vector3 offset;
    Vector3 offset2;

    Vector3 offsetYes;
    [SerializeField] Vector3 offsetValues;


    public static float cameraYRotation;


    void Start()
    {
        offset = new Vector3(player.position.x+offsetValues.x, player.position.y + offsetValues.y, player.position.z + offsetValues.z); //+6 +3 -0.75  +6 +3.5 -3
        offset2 = new Vector3(player.position.x + offsetValues.x, player.position.y + offsetValues.y, player.position.z + offsetValues.z);

        //transform.LookAt(player.position);

    }

    // Update is called once per frame
    void Update()
    {
        cameraYRotation = this.gameObject.transform.eulerAngles.y;


        //transform.RotateAround(player.position, 2 * Time.deltaTime);
        //transform.RotateAround(player.position, new Vector3(0, Input.GetAxis("Mouse X"), 0), rotationSpeed*Time.deltaTime); //new Vector3(0,1,0)
        //transform.LookAt(player);
        //this.transform.position = new Vector3(player.transform.position.x - 0.5f, transform.position.y, player.transform.position.z - 3f);
        //this.transform.position = new Vector3(player.transform.localPosition.x - 0.5f, transform.localPosition.y, player.transform.localPosition.z - 3f);

        //transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);

        //transform.rotation = cameraPos.rotation;


        //transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed); //1

        //transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"),0 , 0) * Time.deltaTime * rotationSpeed);

        //transform.position = cameraPos.position;

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);


        transform.rotation = transform.rotation * Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, Vector3.up); // 1 v2
        //transform.rotation = transform.rotation * Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Vector3.right);
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed *Time.deltaTime, Vector3.up) * offset; //2
        //offset = Quaternion.AngleAxis((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * rotationSpeed * Time.deltaTime, Vector3.up) * offset;



        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, Vector3.up) * offset;


        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Vector3.right) * offset;






        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, Vector3.up) * offset; //2 v2
        //offset2 = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Vector3.right) * offset2;




        //transform.position = player.position +  (offset + offset2)/2; //3

        transform.position = player.position + offset; //3 v2

        //transform.position = player.position + offsetYes;

        //transform.position = player.position + (((offset +offset2).normalized)*5);

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //transform.LookAt(player.position);


        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
/*
         private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
*/