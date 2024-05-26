using UnityEngine;
using System.Collections;

public class DeactivateAfterTime : MonoBehaviour
{
    public float delay = 1f;
    static private bool heartEnabled;
    public GameObject heartDropOnPlayer;

    void OnEnable()
    {
        StartCoroutine(DeactivateAfterDelay());
    }

    public static void EnableHeart()
    {
        heartEnabled = true;
    }

    private void Update() 
    {
        if (heartEnabled)
        {
            heartDropOnPlayer.SetActive(true);
            heartEnabled = false;
            OnEnable();
        }
    }

    IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        heartDropOnPlayer.SetActive(false);
    }
}
