using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRoom : MonoBehaviour
{
    [SerializeField] Animator Bars;
    private int buttonCount;
    private bool doorOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonCount == 2)
        {
            Bars.Play("BarsDown");
            if (!doorOpen)
            {
                SFXController.PlaySFX("Secret", 0.5f);
                doorOpen = true;       
            }
        }
        else
        {
            Bars.Play("BarsUp");
        }
    }

    public void ButtonPressed()
    {
        buttonCount++;
    }

    public void ButtonNotPressed()
    {
        buttonCount--;
    }
}
