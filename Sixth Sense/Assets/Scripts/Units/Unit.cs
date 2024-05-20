using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int strength;
    public int magic;
    public int defense;
    public int resistance;
    public int moveSpeed;

    protected virtual void Start()
    {
        // Ensure current health starts at max health
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    protected virtual void Die()
    {
        // Handle unit death
        Destroy(gameObject);
    }
}
