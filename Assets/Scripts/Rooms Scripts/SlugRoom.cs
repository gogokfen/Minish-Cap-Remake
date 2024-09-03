using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SlugRoom : MonoBehaviour
{
    int pressureButtonCounter = 0;
    public List<UnityEvent> events;
    public List<float> eventDelays;
    public bool sequentialTrigger = true;
    public bool triggerOnce = true;
    public VolumeProfile volumeProfile;
    private void Start() 
    {
        volumeProfile.TryGet<ColorAdjustments>(out ColorAdjustments expo);
        expo.postExposure.value = 1f; //-1 originally
    }
    public void PressureButton()
    {
        pressureButtonCounter++;
        volumeProfile.TryGet<ColorAdjustments>(out ColorAdjustments expo);
        expo.postExposure.value += 0.20f;
        if (pressureButtonCounter == 4)
        {
            if (sequentialTrigger)
            {
                StartCoroutine(TriggerSequentially());
                Movement.Scene(2); //I added so link can't move, original delays were 1.5 and 1.5
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
