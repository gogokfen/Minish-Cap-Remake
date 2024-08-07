using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public Vector3 rotationAxis = Vector3.forward;
    private bool linkInZone;
    private bool linkRiding;
    private float stationTimer;
    public float station;
    [SerializeField] GameObject barrel;
    [SerializeField] GameObject wheel;
    [SerializeField] GameObject player;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Transform seatPosition;
    [SerializeField] GameObject barrelCamera;
    private void Update()
    {
        stationTimer -= Time.deltaTime;
        if (linkInZone && Input.GetKeyDown(KeyCode.Space) && linkRiding == false && !Movement.midAction)
        {
            ActionText.UpdateText("Exit");
            playerAnimator.SetBool("Moving", false);
            playerAnimator.Play("Idle");
            playerAnimator.SetBool("LinkRiding", true);
            player.transform.position = seatPosition.transform.position;
            linkRiding = true;
            Movement.Scene(99999999);
            barrelCamera.SetActive(true);
        }
        else if (linkRiding == true && Input.GetKeyDown(KeyCode.Space))
        {
            ActionText.UpdateText("Ride");
            linkRiding = false;
            playerAnimator.SetBool("LinkRiding", false);
            Movement.Scene(0);
            barrelCamera.SetActive(false);
        }
        if (Input.GetKey(KeyCode.A) && linkRiding && stationTimer <= 0)
        {
            barrel.transform.Rotate(-rotationAxis * rotationSpeed * Time.deltaTime);
            wheel.transform.Rotate(Vector3.left * 10 * Time.deltaTime, Space.World);
            rotationSpeed += 10 * Time.deltaTime;
            Debug.Log(barrel.transform.eulerAngles.x);
            playerAnimator.SetInteger("BarrelTurn", 1);
            if (rotationSpeed >= 40f)
            {
                rotationSpeed = 40f;
            }
        }
        else if (Input.GetKey(KeyCode.D) && linkRiding && stationTimer <= 0)
        {
            barrel.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
            wheel.transform.Rotate(Vector3.right * 10 * Time.deltaTime, Space.World);
            rotationSpeed += 10 * Time.deltaTime;
            Debug.Log(barrel.transform.eulerAngles.x);
            playerAnimator.SetInteger("BarrelTurn", -1);
            if (rotationSpeed >= 40f)
            {
                rotationSpeed = 40f;
            }
        }
        else
        {
            rotationSpeed = 10f;
            playerAnimator.SetInteger("BarrelTurn", 0);
        }

        if ((int)barrel.transform.eulerAngles.x == (station))
        {
            stationTimer = 2f;
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
