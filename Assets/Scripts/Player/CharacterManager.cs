using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Character spawned with {currentHealth}/{maxHealth} health");
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Character took {damageAmount} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        Debug.Log($"Character healed for {healAmount}. Health: {currentHealth}/{maxHealth}");
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        Debug.Log("Character died!");
    }
}
