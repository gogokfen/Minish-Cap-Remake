using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChuChuLegs : MonoBehaviour
{
    [SerializeField] ChuChu chuchu;

    [SerializeField] ParticleSystem gustJarChuChuParticles;

    float succAnimTimer;
    [SerializeField] GameObject chuchuBody;
    [HideInInspector]
    public Material chuchuMat;
    [HideInInspector]

    void Start()
    {
        chuchuMat = chuchuBody.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (chuchu.wakingUp)
        {
            chuchu.wakingUp = false;
            transform.DOScale(Vector3.one, 1); //return to original size
        }
        succAnimTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GustJar" && !chuchu.vulnerable)
        {
            if (transform.localScale.magnitude > 0.80f) //0.85f
            {
                transform.localScale /= 1 + (Time.deltaTime/6f); //Chuchu's legs gradually become smaller

                if (!gustJarChuChuParticles.isPlaying)
                {
                    gustJarChuChuParticles.Play();
                }

                if (succAnimTimer<=0)
                {
                    succAnimTimer = 0.4f; //effect repeats

                    Sequence chuchuSucced = DOTween.Sequence();

                    chuchuSucced.Append(chuchuMat.DOColor(new Color32(100, 100, 100, 125), 0.2f));
                    chuchuSucced.Append(chuchuMat.DOColor(new Color32(255, 255, 255, 255), 0.2f));
                }
            }
            else
            {
                chuchu.vulnerable = true;
                chuchu.Waddle();
                chuchu.anim.SetBool("Waddling", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GustJar" && !chuchu.vulnerable)
        {
            gustJarChuChuParticles.Stop();
        }
    }
}
