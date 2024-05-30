using System.Collections;
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
    public float moveDuration = 0.5f; // Duration for the lerp movement

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

    public virtual void Move(Vector2Int newPosition)
    {
        gameBoardManager.RemoveUnitPosition(currentPosition);
        StartCoroutine(LerpPosition(newPosition, moveDuration));
        currentPosition = newPosition;
        gameBoardManager.SetUnitPosition(newPosition, this);
    }

    protected IEnumerator LerpPosition(Vector2Int targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPosition; // Ensure final position is set
    }

    public Vector2Int GetUnitPosition()
    {
        return currentPosition;
    }
}
