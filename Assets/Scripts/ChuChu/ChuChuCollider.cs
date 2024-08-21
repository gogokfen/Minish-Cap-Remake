using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ChuChuCollider : MonoBehaviour
{
    [SerializeField] ChuChu chuchu;


    bool gotHit;
    float gotHitTimer;
    [SerializeField] GameObject chuchuBody;
    [HideInInspector]
    public Material chuchuMat;
    [HideInInspector]
    public Color32 chuchuColor;

    [SerializeField] Slider hpBar;

    bool startup = true;

    [SerializeField] TextMeshProUGUI bossText;


    void Start()
    {
        startup = true;

        bossText.gameObject.SetActive(true);
        bossText.alpha = 0;

        chuchuMat = chuchuBody.GetComponent<Renderer>().material;
        chuchuColor = new Color32(0, 255, 0, 255);

        hpBar.gameObject.SetActive(true);
        hpBar.value = Mathf.InverseLerp(0, 40, 0); //chuchu.hp
    }

    // Update is called once per frame
    void Update()
    {
        if (startup)
        {
            Movement.Stun(3.5f);
            if (hpBar.value > 0.7f)
            {
                bossText.characterSpacing += Time.deltaTime * 15;
                bossText.alpha += (Time.deltaTime / 2);
            }


            /*
            if (hpBar.value>0.75f)
            {
                fade = true;
            }

            if (hpBar.value>0.5f && !fade)
            {
                bossText.characterSpacing += Time.deltaTime * 15;
                bossText.alpha+= (Time.deltaTime / 2);
            }
            else if (fade)
            {
                bossText.characterSpacing += Time.deltaTime * 15;
                bossText.alpha -= (Time.deltaTime / 2);
            }

            */
            if (hpBar.value < 1)
            {
                hpBar.value += (Time.deltaTime / 7);
            }
            else
            {
                startup = false;
                chuchu.enabled = true;
            }

        }
        else
        {
            if (bossText.alpha > 0)
            {
                bossText.characterSpacing += Time.deltaTime * 15;
                bossText.alpha -= (Time.deltaTime / 2);
            }
            
                
        }

        if (gotHitTimer>=0)
        {
            gotHitTimer -= Time.deltaTime;
            //chuchuMat.SetColor("_BaseColor", chuchuColor);

            /*
            if (gotHitTimer>0.125)
            {
                chuchuColor = new Color32(0, (byte)(0 + (gotHitTimer * 1000)), 0, 255);
            }
            else
            {
                chuchuColor = new Color32(0, (byte)(125 + ((0.125 - gotHitTimer) * 1000)), 0, 255);
            }
            */
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Moveable") //helps chuchu avoid getting out of the map
        {
            //Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Vector3 knockBack = chuchu.transform.position - other.transform.position;
            knockBack.Normalize();
            chuchu.transform.position = new Vector3(chuchu.transform.position.x+ knockBack.x, chuchu.transform.position.y, chuchu.transform.position.z+knockBack.z);
        }

        if (other.tag == "Weapon")
        {
            if (chuchu.fallen)
            {
                Movement.swordHit = true;
                //hitEffect.Play();
                //SFXController.PlaySFX("ChuchuHurt", 1); //add getting hit sound

                chuchu.hp--;

                hpBar.value = Mathf.InverseLerp(0, 40, chuchu.hp);

                gotHitTimer = 0.25f;

                Sequence chuchuHit = DOTween.Sequence();
                chuchuHit.Append(chuchuMat.DOColor(new Color32(175, 0, 0, 255), 0.125f));
                chuchuHit.Append(chuchuMat.DOColor(new Color32(0, 255, 0, 255), 0.125f));

                if (chuchu.hp <= 0)
                {
                    hpBar.gameObject.SetActive(false);
                    Destroy(chuchu.gameObject,1f);
                }
                //Debug.Log(chuchu.hp);
            }
            else
            {
                //SFXController.PlaySFX("swordTINGGGGGG");

                Movement.swordBlocked = true;
                Movement.enemyShielded = true;
                Vector3 tempDirection2 = (Movement.playerPosition - transform.position);
                tempDirection2.Normalize();
                tempDirection2 *= 0.5f;
                Movement.SmallHit(new Vector2(tempDirection2.x, tempDirection2.z), true);
            }
        }

        if (other.tag == "Player")
        {
            if (!chuchu.fallen) //can't hit you when he is down
            {
                Vector3 tempDirection = (Movement.playerPosition - transform.position);
                tempDirection.Normalize();
                tempDirection *= 1.5f;
                Movement.enemyHitAmount = 1;
                Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
            }

        }


        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection.Normalize();
            tempDirection *= 1.5f;
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
        }
    }
}
