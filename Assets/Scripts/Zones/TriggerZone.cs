using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] GameObject activateable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activateable.SetActive(true);
            Destroy(gameObject);
        } 
    }
}
