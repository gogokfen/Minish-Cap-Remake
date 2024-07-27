using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossRoom : MonoBehaviour
{
    private int MinibossCount;
    [SerializeField] GameObject gustJarChest;
    public void MinibossCheck()
    {
        MinibossCount++;
        if (MinibossCount == 2)
        {
            gustJarChest.SetActive(true);
            SFXController.PlaySFX("Secret", 0.5f);
        }
    }
}
