using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] Transform directionalLight;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 300 + Time.time * 0.5f);

        directionalLight.Rotate(-Vector3.up * Time.deltaTime * 0.5f,Space.World);
    }
}
