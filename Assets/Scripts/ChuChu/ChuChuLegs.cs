using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChuChuLegs : MonoBehaviour
{
    [SerializeField] ChuChu chuchu;

    float succAnimTimer;
    [SerializeField] GameObject chuchuBody;
    [HideInInspector]
    public Material chuchuMat;
    [HideInInspector]

    void Start()
    {
        chuchuMat = chuchuBody.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (chuchu.wakingUp)
        {
            chuchu.wakingUp = false;
            transform.DOScale(Vector3.one, 1);
            
            /*
            if (transform.localScale.x<1)
            {
                //transform.localScale *= 1.0015f;
                transform.localScale *= 1+(Time.deltaTime/1.25f);
            }
            */
        }
        succAnimTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar" && !chuchu.vulnerable)
        {
            if (transform.localScale.magnitude > 0.80f) //0.85f
            {
                //transform.localScale /= 1.004f;
                transform.localScale /= 1 + (Time.deltaTime/6f);


                if (succAnimTimer<=0)
                {
                    succAnimTimer = 0.4f;

                    Sequence chuchuSucced = DOTween.Sequence();
                    chuchuSucced.Append(chuchuMat.DOColor(new Color32(0, 125, 0, 255), 0.2f));
                    chuchuSucced.Append(chuchuMat.DOColor(new Color32(0, 255, 0, 255), 0.2f));
                }


            }
            else
            {
                chuchu.vulnerable = true;
                chuchu.Waddle();
            }
            
            
        }
    }
}
