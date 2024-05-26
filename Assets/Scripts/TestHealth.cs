using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public HealthSystem healthSystem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            HealthSystem.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            HealthSystem.Heal(1);
        }
    }
}
