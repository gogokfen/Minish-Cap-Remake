using UnityEngine;

public class Pot : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject heartDropPrefab;

    float animationTime;
    Vector3 originalPos;
    bool lifting;
    [HideInInspector]
    public bool throwing;
    [HideInInspector]
    public Transform playerChild;

    public bool inZone = false;
    bool potUp = false;

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
        if (Input.GetKeyDown(KeyCode.Space) && !throwing && !Movement.potUp && inZone && !Movement.gustCamera) //pot lift
        {
            lifting = true;
            potUp = true;
            animationTime = 0;
            originalPos = transform.position;
            Movement.Stun(0.4f);
            Movement.potUp = true;
            int randomSFX = Random.Range(1, 3);
            switch (randomSFX)
            {
                case 1:
                    SFXController.PlaySFX("LinkLift1", 0.35f);
                    break;
                case 2:
                    SFXController.PlaySFX("LinkLift2", 0.35f);
                    break;
            }
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
            if (succed) // in case launched from gust jar
                transform.Translate(Vector3.forward * 50 * Time.deltaTime);
            else
                transform.Translate((Vector3.forward * 50 + -Vector3.up * 15) * Time.deltaTime);

            if (transform.position.y <= 0)
            {
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
    }

    public void Throw()
    {
        Destroy(gameObject, 3); //making sure pot is destroyed when thrown out of room
        transform.tag = "Weapon";
        ActionText.UpdateText("");
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Movement.playerYRotation, transform.eulerAngles.z);
        throwing = true;
        transform.SetParent(null);
        Movement.throwing = true;
        potUp = false;
        int randomSFX = Random.Range(1, 3);
            switch (randomSFX)
            {
                case 1:
                    SFXController.PlaySFX("LinkThrow1", 0.55f);
                    break;
                case 2:
                    SFXController.PlaySFX("LinkThrow2", 0.55f);
                    break;
            }
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
