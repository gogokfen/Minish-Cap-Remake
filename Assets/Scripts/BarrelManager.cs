using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    public float rotationSpeed = 10;
    public Vector3 rotationAxis = Vector3.forward;
    private bool linkInZone;
    private bool linkRiding;
    private bool canGetOff = true;
    private float stationTimer;
    public float station;

    float rotationAmount;

    [SerializeField] GameObject barrel;
    [SerializeField] GameObject wheel;
    [SerializeField] GameObject player;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Transform seatPosition;
    [SerializeField] GameObject barrelCamera;
    private void Update()
    {
        if (linkRiding)
            stationTimer -= Time.deltaTime;

        if (linkInZone && Input.GetKeyDown(KeyCode.Space) && linkRiding == false)
        {
            ActionText.UpdateText("");
            //ActionText.UpdateText("Exit");
            playerAnimator.SetBool("Moving", false);
            playerAnimator.Play("Idle");
            playerAnimator.SetBool("LinkRiding", true);
            player.transform.position = seatPosition.transform.position;
            player.transform.eulerAngles = new Vector3(0, -90, 0); //making sure link is looking at the wheel
            linkRiding = true;
            Movement.BarrelRiding(true);
            barrelCamera.SetActive(true);
        }
        else if (linkRiding == true && Input.GetKeyDown(KeyCode.Space) && canGetOff)
        {
            ActionText.UpdateText("Ride");
            linkRiding = false;
            playerAnimator.SetBool("LinkRiding", false);
            Movement.BarrelRiding(false);
            barrelCamera.SetActive(false);
        }

        if (stationTimer<=0)
        {
            if (Input.GetKey(KeyCode.A) && linkRiding)
            {
                ActionText.UpdateText("");
                canGetOff = false;

                barrel.transform.Rotate(-rotationAxis * rotationSpeed * Time.deltaTime);

                rotationAmount += (-1 * rotationSpeed * Time.deltaTime);

                wheel.transform.Rotate(Vector3.left * rotationSpeed * 7 * Time.deltaTime, Space.World);
                rotationSpeed *= (1 + (Time.deltaTime / 2f));
                //Debug.Log(barrel.transform.eulerAngles.x);
                playerAnimator.SetInteger("BarrelTurn", 1);
                if (rotationSpeed >= 60f)
                {
                    rotationSpeed = 60f;
                }
            }
            else if (Input.GetKey(KeyCode.D) && linkRiding)
            {
                ActionText.UpdateText("");
                canGetOff = false;

                barrel.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

                rotationAmount += (1 * rotationSpeed * Time.deltaTime);

                wheel.transform.Rotate(Vector3.right * rotationSpeed * 7 * Time.deltaTime, Space.World);
                rotationSpeed *= (1 + (Time.deltaTime / 2f));
                //Debug.Log(barrel.transform.eulerAngles.x);
                playerAnimator.SetInteger("BarrelTurn", -1);
                if (rotationSpeed >= 60f)
                {
                    rotationSpeed = 60f;
                }
            }
            else
            {
                rotationSpeed = 10f;
                playerAnimator.SetInteger("BarrelTurn", 0);
            }

            if ((int)rotationAmount % station >= 0 && (int)rotationAmount % station <=3 && stationTimer <-3) //(int)barrel.transform.eulerAngles.x == (station)
            {
                ActionText.UpdateText("Exit");
                canGetOff = true;
                rotationSpeed = 10;
                stationTimer = 2f;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            ActionText.UpdateText("Ride");
            linkInZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionText.UpdateText("");
            linkInZone = false;
        }
    }
}
