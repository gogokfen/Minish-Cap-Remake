using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuChuShake : MonoBehaviour
{
    [SerializeField] float shakeSpeed;
    void Start()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(60 * Time.deltaTime, 0, 0);
        transform.Translate(Vector3.forward * Time.deltaTime * shakeSpeed);
        shakeSpeed *= 1 + Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection /= 2; //reduce knockback
            Movement.enemyHitAmount = 1;
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
            Destroy(gameObject);
        }

        if (other.tag.Equals("Shield"))
        {
            Movement.enemyShielded = true;
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection /= 2; //reduce knockback
            Movement.SmallHit(new Vector2(tempDirection.x, tempDirection.z));
            Destroy(gameObject);
        }
    }
}
