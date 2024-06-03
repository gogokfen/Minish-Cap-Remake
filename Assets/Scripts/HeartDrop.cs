using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDrop : MonoBehaviour
{
    Rigidbody launchUp;
    public float thrustX = 20f;
    public float thrustY = 20f;
    public float thrustZ = 20f;
    //HealthSystem healthSystem;
    void Start()
    {
        launchUp = GetComponent<Rigidbody>();
        Vector3 force = new Vector3(Random.Range(-thrustX, thrustX), thrustY, Random.Range(-thrustZ, thrustZ));
        launchUp.AddForce(force, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem.Heal(4);
            SFXController.PlaySFX("GetHeart", 1.0f);
            DeactivateAfterTime.EnableHeart();
            Destroy(gameObject);
        }
    }
}
