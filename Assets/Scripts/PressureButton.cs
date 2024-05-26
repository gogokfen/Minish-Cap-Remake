using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    public bool hardButton;
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
        if (hardButton)
        {
            animator.SetTrigger("HardPress");
            hardButtonPressed.Invoke();
        }

        if (!hardButton)
        {
            animator.SetBool("SoftPress", true);
            softButtonPressed.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        animator.SetBool("SoftPress", false);
        softButtonReleased.Invoke();
    }
}
