using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    public bool hardButton;
    bool pressed;
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent hardButtonPressed;
    [SerializeField] UnityEvent softButtonPressed;
    [SerializeField] UnityEvent softButtonReleased;
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
        if (other.tag == "Player" && hardButton && !pressed)
        {
            animator.SetTrigger("HardPress");
            hardButtonPressed.Invoke();
            pressed = true;
        }

        if (other.tag == "Player" && !hardButton && !pressed)
        {
            animator.SetBool("SoftPress", true);
            softButtonPressed.Invoke();
            pressed = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag == "Player" && !hardButton && pressed)
        {
        animator.SetBool("SoftPress", false);
        softButtonReleased.Invoke();
        pressed = false;
        }
    }
}
