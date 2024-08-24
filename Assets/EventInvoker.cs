using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInvoker : MonoBehaviour
{
    public UnityEvent Event;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Event.Invoke();
            Destroy(gameObject);
        }
    }
}
