using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image[] hearts;
    private Animator[] heartAnimators;
    public static int maxHealth;
    [SerializeField] int maxHP;
    public static int currentHealth;
    [SerializeField] int currentHP;
    public static bool updatedHearts;

    void Start()
    {
        maxHealth = maxHP;
        currentHealth = currentHP;

        heartAnimators = new Animator[hearts.Length];
        for (int i = 0; i < hearts.Length; i++)
        {
            heartAnimators[i] = hearts[i].GetComponent<Animator>();
        }

        UpdateHearts();
    }

    public static void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        updatedHearts = true;
    }

    public static void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        updatedHearts = true;
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

            hearts[i].transform.localScale = Vector3.one;

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


            heartAnimators[i].SetBool("IsCurrentHeart", false);
        }

        int currentHeartIndex = (currentHealth - 1) / 4;
        if (currentHeartIndex >= 0 && currentHeartIndex < hearts.Length)
        {
            hearts[currentHeartIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1);


            heartAnimators[currentHeartIndex].SetBool("IsCurrentHeart", true);
        }
    }
}
