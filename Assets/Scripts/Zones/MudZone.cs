using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudZone : MonoBehaviour
{
    float suctionWindup;

    bool dying = false;
    bool played = false;

    Material sporeMaterial;
    private float dissolveValue = -1;
    private float hightValue = 0;

    Vector3 originalPosition;
    

    private void Start()
    {
        sporeMaterial = GetComponent<Renderer>().material;
        originalPosition = transform.position;
    }
    private void Update()
    {
        if (dying)
        { 
            transform.position = Vector3.Lerp(originalPosition, new Vector3 (Movement.gustJarPos.x, Movement.gustJarPos.y+0.5f, Movement.gustJarPos.z), (hightValue/2)); //Movement.gustJarPos
            //transform.localScale = new Vector3((1 - hightValue/2), (1 - hightValue / 2), (1 - hightValue / 2));
            transform.localScale /= (1 + Time.deltaTime*4);

            dissolveValue += Time.deltaTime *3f;
            //sporeMaterial.SetFloat("DissolveHight",dissolveValue );
            hightValue += Time.deltaTime *3;
            //sporeMaterial.SetFloat("HightDiff", hightValue);

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
                //SFXController.PlaySFX("GustJarThump", 0.5f);
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
