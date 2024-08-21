using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform teleportPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player" && Chest.gotGotJar)
        {
            player.transform.position = teleportPoint.transform.position;
        }
    }
}
