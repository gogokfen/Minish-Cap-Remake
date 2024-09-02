using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuChuTargetedSpit : MonoBehaviour
{
    [SerializeField] float spitSpeed;
    [HideInInspector]
    public Transform spitTarget;

    [SerializeField] Transform GFX;

    CapsuleCollider spitCollider;

    Quaternion lastRot;
    private void Start()
    {
        transform.LookAt(new Vector3(spitTarget.position.x, spitTarget.position.y + 1.5f, spitTarget.position.z));

        spitCollider = GetComponent<CapsuleCollider>();

        Destroy(gameObject, 1);
    }

    void Update()
    {
        if (GFX.transform.localScale.z<=1) //making the spit gradually bigger for better visual and reaction time
        {
            GFX.transform.localScale = new Vector3(GFX.transform.localScale.x, GFX.localScale.y, GFX.localScale.z * (1 + Time.deltaTime));
        }
        if (spitCollider.height<5.5)
        {
            spitCollider.height += (2.75f * Time.deltaTime); //5.5
        }

        lastRot = transform.rotation;
        transform.LookAt(new Vector3(spitTarget.position.x,spitTarget.position.y+1.5f,spitTarget.position.z));

        transform.rotation = Quaternion.Lerp(lastRot, transform.rotation, Time.deltaTime*5f ); //0.005f the spit slowly turns towards it's target

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
