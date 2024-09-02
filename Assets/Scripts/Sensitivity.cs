using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensitivity : MonoBehaviour
{
    [SerializeField] CameraRotator CR;

    Slider sensitivitySlider;
    void Start()
    {
        sensitivitySlider = GetComponent<Slider>();
        sensitivitySlider.value = Mathf.InverseLerp(0.25f, 2, CR.sens);
    }

    public void AdjustSensitivity(float newSensitivity)
    {
        CR.sens = Mathf.Lerp(0.25f, 2, newSensitivity);
    }
}
