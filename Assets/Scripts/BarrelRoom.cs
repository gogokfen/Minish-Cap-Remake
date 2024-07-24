using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarrelRoom : MonoBehaviour
{
    //private bool barrelDropped = false;
    private int buttonsClicked = 0;
    //public GameObject barrel;
    //[SerializeField] UnityEvent barrelDropEvent;
    // Start is called before the first frame update
    [SerializeField] GameObject barrelTriggerOne;
    [SerializeField] GameObject barrelTriggerTwo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DropTheBarrel()
    {
        buttonsClicked++;
        if (buttonsClicked == 2)
        {
            //barrel.transform.position += new Vector3 (0, -3, 0);
            //barrelDropEvent.Invoke();
            barrelTriggerOne.SetActive(true);
            barrelTriggerTwo.SetActive(true);
            SFXController.PlaySFX("Secret", 0.5f);
        }
    }
}