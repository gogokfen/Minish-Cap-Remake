using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform teleportPoint;
    public bool on;
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player" && (Chest.gotGotJar || on))
        {
            player.transform.position = teleportPoint.transform.position;
        }
    }
}
