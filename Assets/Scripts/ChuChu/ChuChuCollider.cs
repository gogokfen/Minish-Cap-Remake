using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public ParticleSystem hitEffect;


    void Start()
    {
        chuchuMat = chuchuBody.GetComponent<Renderer>().material;
        chuchuColor = new Color32(0, 255, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
        if (gotHitTimer>=0)
        {
            gotHitTimer -= Time.deltaTime;
            chuchuMat.SetColor("_BaseColor", chuchuColor);

            if (gotHitTimer>0.125)
            {
                chuchuColor = new Color32(0, (byte)(0 + (gotHitTimer * 1000)), 0, 255);
            }
            else
            {
                chuchuColor = new Color32(0, (byte)(125 + ((0.125 - gotHitTimer) * 1000)), 0, 255);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            if (chuchu.fallen)
            {
                hitEffect.Play();
                //SFXController.PlaySFX("ChuchuHurt", 1); //add getting hit sound

                chuchu.hp--;
                gotHitTimer = 0.25f;
                if (chuchu.hp <= 0)
                {
                    Destroy(chuchu.gameObject,1f);
                }
                //Debug.Log(chuchu.hp);
            }
        }

        if (other.tag == "Player")
        {
            if (!chuchu.fallen) //can't hit you when he is down
            {
                Vector3 tempDirection = (Movement.playerPosition - transform.position);
                Movement.enemyHitAmount = 1;
                Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
            }

        }


        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
        }
    }
}
