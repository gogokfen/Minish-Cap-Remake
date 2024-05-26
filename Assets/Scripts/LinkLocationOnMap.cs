using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinkLocationOnMap : MonoBehaviour
{
    [SerializeField] UnityEvent changeRoom;
    private void OnTriggerEnter(Collider other) 
    {
        changeRoom.Invoke();
    }
}
