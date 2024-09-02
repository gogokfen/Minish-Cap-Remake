using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotCollider : MonoBehaviour
{
    [SerializeField] Pot pot;
    float suctionWindup;
    float angleX;
    float angleY;

    bool exploding;

    private BoxCollider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (Movement.gustJarUp == false && suctionWindup >= 1)
        {
            Movement.succed = false;
            suctionWindup = 0;
            pot.Throw(angleX, angleY);
            SFXController.PlaySFX("GustJarSuctionPot", 0.5f);

            triggerCollider.tag = "Weapon"; //throwing the pot making it a weapon
            triggerCollider.size = new Vector3(0.1f, 0.1f, 0.1f); //to prevent it from hitting things accidently
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            ActionText.UpdateText("");
            pot.Explode();
        }
        if (other.tag == "Player")
        {
            pot.inZone = true;
            pot.playerChild = other.transform;
            ActionText.UpdateText("Lift");
        }

        if (pot.throwing && !exploding) //&& other.tag == "Moveable"
        {
            exploding = true; //prevents from multiple explosions happening simutaniously
            pot.Explode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar" && (pot.succed || !Movement.succed))
        {
            suctionWindup += Time.deltaTime;
            if (suctionWindup>=1)
            {
                if (!pot.succed)
                    SFXController.PlaySFX("GustJarThump", 0.5f);

                pot.succed = true;
                Movement.succed = true;

                pot.potPhysicalCol.enabled = false;
                pot.transform.position = Vector3.Lerp(pot.transform.position, Movement.gustJarPos, suctionWindup - 1);
                
                angleX = other.transform.eulerAngles.x;
                angleY = other.transform.eulerAngles.y;

            }
            else //shake effect
            {
                Vector3 randomShake = new Vector3(Random.Range(-5f, 5f), Random.Range(0, 1f), Random.Range(-5f, 5f)); //in case no animation
                pot.transform.Translate(randomShake * Time.deltaTime);
            }
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
