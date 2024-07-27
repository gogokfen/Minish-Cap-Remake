using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MulldozerRoom : MonoBehaviour
{
    [SerializeField] GameObject[] mulldozers;
    private int mulldozerDeathCount;
    public UnityEvent deathToAllMulldozers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyedCheck()
    {
        mulldozerDeathCount++;
        if (mulldozerDeathCount == mulldozers.Length)
        {
            SFXController.PlaySFX("Secret", 0.5f);
            deathToAllMulldozers.Invoke();
        }
    }
}
