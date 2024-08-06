using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image[] hearts;
    private Animator[] heartAnimators;
    private int[] originalSiblingIndexes;
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
        originalSiblingIndexes = new int[hearts.Length];

        for (int i = 0; i < hearts.Length; i++)
        {
            heartAnimators[i] = hearts[i].GetComponent<Animator>();
            originalSiblingIndexes[i] = hearts[i].transform.GetSiblingIndex();
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
            Image childImage = hearts[i].transform.GetChild(0).GetComponent<Image>();
            float fillAmount = 0f;
            if (currentHealth >= heartIndex + 4)
            {
                fillAmount = 1f;
            }
            else if (currentHealth >= heartIndex + 3)
            {
                fillAmount = 0.75f;
            }
            else if (currentHealth >= heartIndex + 2)
            {
                fillAmount = 0.5f;
            }
            else if (currentHealth >= heartIndex + 1)
            {
                fillAmount = 0.25f;
            }
            childImage.fillAmount = fillAmount;
            heartAnimators[i].SetBool("IsCurrentHeart", false);
            hearts[i].transform.SetSiblingIndex(originalSiblingIndexes[i]);
        }

        int currentHeartIndex = (currentHealth - 1) / 4;
        if (currentHeartIndex >= 0 && currentHeartIndex < hearts.Length)
        {
            
            heartAnimators[currentHeartIndex].SetBool("IsCurrentHeart", true);
            hearts[currentHeartIndex].transform.SetSiblingIndex(hearts.Length - 1);
        }
    }
}
