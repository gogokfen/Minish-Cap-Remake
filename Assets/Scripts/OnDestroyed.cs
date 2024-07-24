using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDestroyed : MonoBehaviour
{
    [SerializeField] UnityEvent onDestoyedEvent;
    void OnDestroy()
    {
        onDestoyedEvent.Invoke();
    }
}
