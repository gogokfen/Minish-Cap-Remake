using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudZone : MonoBehaviour
{
    float suctionWindup;

    bool dying = false;
    bool played = false;

    private float hightValue = 0;

    Vector3 originalPosition;
    

    private void Start()
    {
        originalPosition = transform.position;
    }
    private void Update()
    {
        if (dying) //was intended to use with shader, turned out no fitting the game visuals so scrapped
        { 
            transform.position = Vector3.Lerp(originalPosition, new Vector3 (Movement.gustJarPos.x, Movement.gustJarPos.y+0.5f, Movement.gustJarPos.z), (hightValue/2)); //Movement.gustJarPos
            transform.localScale /= (1 + Time.deltaTime*4);

            hightValue += Time.deltaTime *3;

            if (!played && (transform.position - Movement.gustJarPos ).magnitude<0.6f)
            {
                SFXController.PlaySFX("GustJarThump", 0.6f);
                played = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Movement.mud = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar" && !dying)
        {
            suctionWindup += Time.deltaTime;
            if (suctionWindup>0.5f)
            {
                dying = true;
                Movement.dustSucced = true;
                Destroy(gameObject,2);
            }
        }
        if (other.tag == "Player" && dying)
        {
            Movement.mud = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Movement.mud = false;
        }
    }
}
