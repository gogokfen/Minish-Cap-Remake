using UnityEngine;

public class Pot : MonoBehaviour
{
    public float detectionDistance = 2.0f;
    public GameObject particlePrefab;
    public GameObject heartDropPrefab;
    public LayerMask playerLayer;

    private void Update()
    {
        Vector3 boxCenter = transform.position;
        Vector3 boxHalfExtents = new Vector3(detectionDistance, detectionDistance, detectionDistance);
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, playerLayer);
        foreach (var hit in hits)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(gameObject);
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
                Instantiate(heartDropPrefab, transform.position + new Vector3 (0f, 0.25f, 0f), Quaternion.identity);
                break;
            }
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

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position;
        Vector3 boxHalfExtents = new Vector3(detectionDistance, detectionDistance, detectionDistance);  // Adjust size as needed
        Gizmos.DrawWireCube(boxCenter, boxHalfExtents * 2);
    }
}
