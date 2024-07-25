using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlugRoom : MonoBehaviour
{
    int pressureButtonCounter = 0;
    public List<UnityEvent> events;
    public List<float> eventDelays;
    public bool sequentialTrigger = true;
    public bool triggerOnce = true;
    //private bool triggered = false;
    //[SerializeField] UnityEvent slugRoomEvent;
    public void PressureButton()
    {
        pressureButtonCounter++;
        if (pressureButtonCounter == 4)
        {
            if (sequentialTrigger)
            {
                StartCoroutine(TriggerSequentially());
            }
            else
            {
                TriggerAllAtOnce();
            }
            //slugRoomEvent.Invoke();
        }
    }


    private IEnumerator TriggerSequentially()
    {
        for (int i = 0; i < events.Count; i++)
        {
            events[i].Invoke();

            // Check if a delay is specified for the current event
            if (i < eventDelays.Count)
            {
                yield return new WaitForSeconds(eventDelays[i]);
            }
            else
            {
                yield return null;
            }
        }

        if (triggerOnce)
        {
            //triggered = true;
            Destroy(gameObject);
        }
    }

    private void TriggerAllAtOnce()
    {
        for (int i = 0; i < events.Count; i++)
        {
            events[i].Invoke();

            // Check if a delay is specified for the current event
            if (i < eventDelays.Count)
            {
                // Wait for the specified delay time
                WaitForSeconds delay = new WaitForSeconds(eventDelays[i]);
                StartCoroutine(DelayedDestroy(delay));
            }
        }

        if (triggerOnce)
        {
            //triggered = true;
            Destroy(gameObject);
        }
    }

    private IEnumerator DelayedDestroy(WaitForSeconds delay)
    {
        yield return delay;
        Destroy(gameObject);
    }

    public void playSFX()
    {
        SFXController.PlaySFX("Secret", 0.5f);
    }
}