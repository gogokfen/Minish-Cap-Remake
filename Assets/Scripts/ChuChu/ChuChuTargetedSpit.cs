using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuChuTargetedSpit : MonoBehaviour
{
    [SerializeField] float spitSpeed;
    [HideInInspector]
    public Transform spitTarget;

    Quaternion lastRot;
    private void Start()
    {
        transform.LookAt(new Vector3(spitTarget.position.x, spitTarget.position.y + 1.5f, spitTarget.position.z));

        Destroy(gameObject, 1);
    }

    void Update()
    {
        lastRot = transform.rotation;
        transform.LookAt(new Vector3(spitTarget.position.x,spitTarget.position.y+1.5f,spitTarget.position.z));

        transform.rotation = Quaternion.Lerp(lastRot, transform.rotation, Time.deltaTime*5f ); //0.005f


        transform.Translate(Vector3.forward * Time.deltaTime * spitSpeed);
        spitSpeed *= 1 + Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Vector3 tempDirection = (Movement.playerPosition - transform.position);
            tempDirection /= 2; //reduce knockback
            Movement.enemyHitAmount = 3;
            Movement.SmallHit(new Vector2(tempDirection.x,tempDirection.z));
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
