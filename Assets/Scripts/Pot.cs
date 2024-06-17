using UnityEngine;

public class Pot : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject heartDropPrefab;

    float animationTime;
    Vector3 originalPos;
    //Vector2 throwDirection = new Vector2 (0,1);
    bool lifting;
    [HideInInspector]
    public bool throwing;
    [HideInInspector]
    public Transform playerChild;

    public bool inZone = false;
    bool potUp = false;

    Ray raycast;
    RaycastHit player;
    [SerializeField] LayerMask mask;

    [HideInInspector]
    public BoxCollider potPhysicalCol;

    [HideInInspector]
    public bool succed = false;

    private void Start()
    {
        potPhysicalCol = GetComponent<BoxCollider>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && !throwing && !Movement.potUp && inZone)
        {
            lifting = true;
            potUp = true;
            animationTime = 0;
            originalPos = transform.position;
            Movement.Stun(0.4f);
            Movement.potUp = true;
        }
        if (lifting)
        {
            animationTime += (Time.deltaTime *2f);
            if (animationTime >= 1)
                lifting = false;
            transform.position = Vector3.Lerp(originalPos, new Vector3(Movement.playerPosition.x, Movement.playerPosition.y + 2.5f, Movement.playerPosition.z), animationTime);
            transform.SetParent(playerChild);
        }

        if (throwing)
        {
            //transform.Translate((Vector3.forward + -Vector3.up) * Time.deltaTime * 20);
            if (succed) // in case launched from gust jar
                transform.Translate(Vector3.forward * 50 * Time.deltaTime);
            else
                transform.Translate((Vector3.forward * 50 + -Vector3.up * 15) * Time.deltaTime);

            if (transform.position.y <= 0)
            {
                //potUp = false;
                Explode();
            }
        }

        if (potUp && !lifting) //|| Input.GetKeyDown(KeyCode.Space) Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) 
        {
            ActionText.UpdateText("Throw");

            if ((Input.GetKeyDown(KeyCode.Space)))
            {
                Throw();
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

    public void Throw()
    {
        Destroy(gameObject, 3); //making sure pot is destroyed when thrown out of room
        transform.tag = "Weapon";
        ActionText.UpdateText("");
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Movement.playerYRotation, transform.eulerAngles.z);
        throwing = true;
        transform.SetParent(null);
        //Movement.potUp = false;
        Movement.throwing = true;
        potUp = false;
    }
    //overload
    public void Throw(float angleX,float angleY)
    {
        Destroy(gameObject, 3);
        transform.tag = "Weapon";
        ActionText.UpdateText("");
        transform.eulerAngles = new Vector3(angleX, angleY, transform.eulerAngles.z);
        throwing = true;
        transform.SetParent(null);
        potUp = false;
    }

    public void Explode()
    {
        Movement.potUp = false;
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        if (Random.Range(1, HealthSystem.currentHealth) == 1)
        {
            Instantiate(heartDropPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
