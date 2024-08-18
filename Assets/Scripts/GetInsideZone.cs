using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInsideZone : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        other.transform.position = transform.position;
    }
}
