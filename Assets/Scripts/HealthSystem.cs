using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image[] hearts;
    public static int maxHealth;
    [SerializeField] int maxHP;
    public static int currentHealth;
    [SerializeField] int currentHP;
    public static bool updatedHearts;


    void Start()
    {
        maxHealth = maxHP;
        currentHealth = currentHP;
        UpdateHearts();
    }

    public static void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        updatedHearts = true;
        //UpdateHearts();
    }

    public static void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        updatedHearts = true;
        //UpdateHearts();
    }

    private void Update() 
    {
        if (updatedHearts)
        {
            UpdateHearts();
            updatedHearts = false;
        }
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartIndex = i * 4;
            if (currentHealth >= heartIndex + 4)
            {
                hearts[i].fillAmount = 1f;
            }
            else if (currentHealth >= heartIndex + 3)
            {
                hearts[i].fillAmount = 0.75f;
            }
            else if (currentHealth >= heartIndex + 2)
            {
                hearts[i].fillAmount = 0.5f;
            }
            else if (currentHealth >= heartIndex + 1)
            {
                hearts[i].fillAmount = 0.25f;
            }
            else
            {
                hearts[i].fillAmount = 0f;
            }
        }
    }
}
