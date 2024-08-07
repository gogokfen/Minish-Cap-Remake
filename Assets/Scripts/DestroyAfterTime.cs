using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 5.0f;  // Time in seconds before the object is destroyed

    private void Start()
    {
        // Schedule the destruction of the object
        Destroy(gameObject, lifetime);
    }
}
