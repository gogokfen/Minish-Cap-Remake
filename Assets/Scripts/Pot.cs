using UnityEngine;

public class Pot : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject heartDropPrefab;

    float animationTime;
    Vector3 originalPos;
    //Vector2 throwDirection = new Vector2 (0,1);
    bool lifting;
    bool throwing;
    [HideInInspector]
    public Transform playerChild;

    public bool inZone = false;
    bool potUp = false;

    Ray raycast;
    RaycastHit player;
    [SerializeField] LayerMask mask;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !throwing && !Movement.potUp && inZone)
        {
            lifting = true;
            potUp = true;
            animationTime = 0;
            originalPos = transform.position;
            Movement.Stun(0.15f);
            Movement.potUp = true;
        }
        if (lifting)
        {
            animationTime += (Time.deltaTime *6.65f);
            if (animationTime >= 1)
                lifting = false;
            transform.position = Vector3.Lerp(originalPos, new Vector3(Movement.playerPosition.x, Movement.playerPosition.y + 2.5f, Movement.playerPosition.z), animationTime);
            transform.SetParent(playerChild);
        }

        if (throwing)
        {
            //transform.Translate((Vector3.forward + -Vector3.up) * Time.deltaTime * 20);
            transform.Translate((Vector3.forward * 50 + -Vector3.up *15) *Time.deltaTime );
            if (transform.position.y <= 0)
            {
                Movement.potUp = false;
                //potUp = false;
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
                Instantiate(heartDropPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
                Destroy(gameObject);
            }
        }

        if (potUp && !lifting) //|| Input.GetKeyDown(KeyCode.Space) Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) 
        {
            ActionText.UpdateText("Throw");

            if ((Input.GetKeyDown(KeyCode.Space)))
            {
                ActionText.UpdateText("");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Movement.playerYRotation, transform.eulerAngles.z);
                throwing = true;
                transform.SetParent(null);
                //Movement.potUp = false;
                potUp = false;
            }

        }

        //4 directions raycast check
        /*
        if (Physics.Raycast(transform.position,Vector3.forward, out player, 1, mask)) //north
        {
            //Debug.Log("north");
        }
        if (Physics.Raycast(transform.position, -Vector3.forward, out player, 1, mask)) //south
        {
            //Debug.Log("south");
        }
        if (Physics.Raycast(transform.position, Vector3.right, out player, 1, mask)) //east
        {
            //Debug.Log("east");
        }
        if (Physics.Raycast(transform.position, -Vector3.right, out player, 1, mask)) //west
        {
            //Debug.Log("west");
        }
        */
    }


}
