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

    protected Vector2Int currentPosition;
    protected GameBoardManager gameBoardManager;
    

    protected virtual void Start()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        currentHealth = maxHealth;
        currentPosition = Vector2Int.RoundToInt(transform.position);
        gameBoardManager.SetUnitPosition(currentPosition, this);
    }

    public virtual void PhysicalAttack(Unit target, int attackPower)
    {
        int damage = attackPower + strength - target.defense;
        target.TakeDamage(damage);
    }

    public virtual void MagicalAttack(Unit target, int attackPower)
    {
        int damage = attackPower + magic - target.resistance;
        target.TakeDamage(damage);
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            damage = 1;
        }
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
        gameBoardManager.RemoveUnitPosition(currentPosition);
        Destroy(gameObject);
    }

    public void Move(Vector2Int newPosition)
    {
        gameBoardManager.RemoveUnitPosition(currentPosition);
        currentPosition = newPosition;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        gameBoardManager.SetUnitPosition(newPosition, this);
    }

    public Vector2Int GetUnitPosition()
    {
        return currentPosition;
    }
}
