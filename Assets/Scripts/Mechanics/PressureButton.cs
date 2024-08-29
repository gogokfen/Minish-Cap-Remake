using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;

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
        if ((other.tag == "Player" || other.tag == "Moveable") && hardButton && !pressed)
        {
            animator.SetTrigger("HardPress");
            hardButtonPressed.Invoke();
            pressed = true;

            SFXController.PlaySFX("PressurePlate", 1f);
        }

        /**
        if ((other.tag == "Player" || other.tag == "Moveable") && !hardButton && !pressed)
        {
            animator.SetBool("SoftPress", true);
            softButtonPressed.Invoke();
            pressed = true;
        }
        */
    }

    private void OnTriggerStay(Collider other) // making sure the button stays pressed in case player gets out and pillar goes in simutatiosly
    {
        if ((other.tag == "Player" || other.tag == "Moveable") && !hardButton && !pressed)
        {
            animator.SetBool("SoftPress", true);
            softButtonPressed.Invoke();
            pressed = true;

            SFXController.PlaySFX("PressurePlate", 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "Moveable") && !hardButton && pressed)
        {
            animator.SetBool("SoftPress", false);
            softButtonReleased.Invoke();
            pressed = false;

            SFXController.PlaySFX("PressurePlateUp", 1f);
        }
    }

    public void SelfDestruct()
    {
        Destroy(this);
    }
}
