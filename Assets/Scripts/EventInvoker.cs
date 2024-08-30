using UnityEngine;
using UnityEngine.Events;
public class EventInvoker : MonoBehaviour
{
    public KeyCode key;
    public bool inputRequired;
    public UnityEvent Event;
    bool inside;
    private void Update()
    {
        if (inputRequired)
        {
            if (Input.GetKeyDown(key) && inside)
            {
                Event.Invoke();
                Destroy(gameObject);
            }
        }
        else if (inside)
        {
            Event.Invoke();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inside = false;
        }
    }

    public void EventInvoke()
    {
        Event.Invoke();
        //Destroy(gameObject);
    }
}
