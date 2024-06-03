using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotCollider : MonoBehaviour
{
    [SerializeField] Pot pot;
    float suctionWindup;
    float angleX;
    float angleY;
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

        if (pot.throwing && other.tag == "Moveable")
        {
            pot.Explode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar")
        {
            suctionWindup += Time.deltaTime;
            if (suctionWindup>=1)
            {
                pot.potPhysicalCol.enabled = false;
                //pot.transform.position = new Vector3(Movement.playerPosition.x,Movement.playerPosition.y+1f,Movement.playerPosition.z);
                pot.transform.position = Vector3.Lerp(pot.transform.position,new Vector3(Movement.playerPosition.x, Movement.playerPosition.y + 1f, Movement.playerPosition.z),suctionWindup-1);
                angleX = other.transform.eulerAngles.x;
                angleY = other.transform.eulerAngles.y;
            }
        }

        if (Movement.gustJarUp == false && suctionWindup>=1)
        {
            pot.succed = true;
            suctionWindup = 0;
            pot.Throw(angleX,angleY);

            //pot.potPhysicalCol.enabled = true;
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
