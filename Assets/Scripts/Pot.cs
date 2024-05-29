using UnityEngine;

public class Pot : MonoBehaviour
{
    public float detectionDistance = 2.0f;
    public GameObject particlePrefab;
    public GameObject heartDropPrefab;
    public LayerMask playerLayer;

    float animationTime;
    Vector3 originalPos;
    //Vector2 throwDirection = new Vector2 (0,1);
    bool lifting;
    bool throwing;
    Transform playerChild;

    private void Update()
    {
        Vector3 boxCenter = transform.position;
        Vector3 boxHalfExtents = new Vector3(detectionDistance, detectionDistance, detectionDistance);
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, playerLayer);
        foreach (var hit in hits)
        {
            if (Input.GetKeyDown(KeyCode.E) && !throwing)
            {
                lifting = true;
                animationTime = 0;
                originalPos = transform.position;
                Movement.Stun(1);
                Movement.potUp = true;
                //transform.SetParent(tra)

                /*
                Destroy(gameObject);
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
                Instantiate(heartDropPrefab, transform.position + new Vector3 (0f, 0.25f, 0f), Quaternion.identity);
                break;
                */
            }
        }
        if (lifting)
        {
            animationTime += Time.deltaTime;
            if (animationTime >= 1)
                lifting = false;
            transform.position = Vector3.Lerp(originalPos, new Vector3(Movement.playerPosition.x,Movement.playerPosition.y+3,Movement.playerPosition.z), animationTime);
            transform.SetParent(playerChild);
            //playerChild.SetParent(transform);
        }

        if (throwing)
        {
            //transform.Translate(Vector3.right * Time.deltaTime * 25);
            //transform.Translate(new Vector3(1, 0, -1) * Time.deltaTime * 25);
            //transform.Translate(Movement.playerYRotation);
            //Debug.Log(Movement.playerYRotation);
            //Debug.Log(Quaternion.AngleAxis(Movement.playerYRotation, Vector3.forward));
            //transform.Translate(new Vector3(throwDirection.y,0, throwDirection.x) * Time.deltaTime * 1);
            //transform.Translate(Quaternion.AngleAxis(Movement.playerYRotation, Vector3.forward) * Vector2.right *Time.deltaTime);

            //transform.Translate(-Vector3.up * Time.deltaTime * 20);
            //transform.Translate(-Vector3.forward * Time.deltaTime * 10);
            transform.Translate(Vector3.forward * Time.deltaTime *20 );
            transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime*5), transform.position.z);
            

            if (transform.position.y <= 0)
            {
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
                Instantiate(heartDropPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
                Destroy(gameObject);
            }
        }





        if (Movement.potUp && !lifting && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E)))
        {
            //throwDirection = Vector3.right * Movement.playerYRotation;
            //throwDirection = Vector3.
            //Debug.Log("original throw dir:" + throwDirection);
            //transform.rotation = Quaternion.identity;

            //throwDirection = new Vector2(throwDirection.x * Mathf.Cos(Movement.playerYRotation) - throwDirection.y * Mathf.Sin(Movement.playerYRotation)
            //    , throwDirection.x * Mathf.Sin(Movement.playerYRotation) + throwDirection.y * Mathf.Cos(Movement.playerYRotation));

            //Debug.Log("new throw dir:" + throwDirection);

            //transform.eulerAngles = new Vector3()
            transform.eulerAngles = new Vector3(0, Movement.playerYRotation, transform.eulerAngles.y);
            throwing = true;
            transform.SetParent(null);

            Movement.potUp = false;
        }


    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Weapon")
        {
            Destroy(gameObject);
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Instantiate(heartDropPrefab, transform.position + new Vector3 (0f, 0.25f, 0f), Quaternion.identity);     
        }
        if (other.tag == "Player")
        {
            //Debug.Log("working");
            playerChild = other.transform;
            //transform.rotation = other.transform.rotation;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position;
        Vector3 boxHalfExtents = new Vector3(detectionDistance, detectionDistance, detectionDistance);  // Adjust size as needed
        Gizmos.DrawWireCube(boxCenter, boxHalfExtents * 2);
    }
}
