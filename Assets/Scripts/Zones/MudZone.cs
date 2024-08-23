using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudZone : MonoBehaviour
{
    float suctionWindup;

    bool dying = false;

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
            transform.position = Vector3.Lerp(originalPosition, Movement.gustJarPos,(hightValue/2));
            //transform.localScale = new Vector3((1 - hightValue/2), (1 - hightValue / 2), (1 - hightValue / 2));
            transform.localScale /= (1 + Time.deltaTime*4);

            dissolveValue += Time.deltaTime *3f;
            //sporeMaterial.SetFloat("DissolveHight",dissolveValue );
            hightValue += Time.deltaTime *3;
            //sporeMaterial.SetFloat("HightDiff", hightValue);
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Movement.mud = false;
        }

    }
}
