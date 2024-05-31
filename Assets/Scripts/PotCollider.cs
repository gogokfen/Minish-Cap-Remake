using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotCollider : MonoBehaviour
{
    [SerializeField] PotAlt pot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Destroy(pot.gameObject);
            Instantiate(pot.particlePrefab, transform.position, Quaternion.identity);
            Instantiate(pot.heartDropPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
        }
        if (other.tag == "Player")
        {
            pot.inZone = true;
            pot.playerChild = other.transform;
            ActionText.UpdateText("Lift");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pot.inZone = false;
            ActionText.UpdateText("");
        }
    }
}
