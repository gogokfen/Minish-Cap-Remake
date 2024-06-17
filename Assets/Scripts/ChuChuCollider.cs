using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuChuCollider : MonoBehaviour
{
    [SerializeField] ChuChu chuchu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            if (chuchu.vulnerable)
            {
                chuchu.hp--;
                if (chuchu.hp<=0)
                {
                    Destroy(chuchu.gameObject);
                }
                //Debug.Log(chuchu.hp);
            }
        }

        if (other.tag == "Player")
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
        }


        if (other.tag == "Shield")
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
        }
    }
}
