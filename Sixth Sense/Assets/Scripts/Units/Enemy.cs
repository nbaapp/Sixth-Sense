using UnityEngine;
using System.Collections.Generic;

public abstract class Enemy : Unit
{
    protected PlayerUnit playerUnit;
    protected TurnManager turnManager;
    protected List<Vector2Int> targetedTiles = new List<Vector2Int>();
    public GameObject healthBarPrefab;
    private HealthBar healthBar;
    private EnemySpawner enemySpawner;
    private SFX sfx;

    private bool hitPlayer = false;
    private bool hitEnemy = false;

    protected virtual void Awake()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        playerUnit = FindObjectOfType<PlayerUnit>();
        turnManager = FindObjectOfType<TurnManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        sfx = FindObjectOfType<SFX>();
    }

    protected override void Start()
    {
        base.Start();
        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(healthBarPrefab, new Vector3(transform.position.x,
                                        transform.position.y - 0.35f, transform.position.z), Quaternion.identity);
            hb.transform.SetParent(transform);
            healthBar = hb.GetComponentInChildren<HealthBar>();
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    protected override void Die()
    {
        ClearHighlights();
        enemySpawner.EnemyDied();
        base.Die();
    }

    public abstract void SelectAction();

    protected virtual void ExecuteAction()
    {
        hitPlayer = false;
        hitEnemy = false;
        foreach (Vector2Int tile in targetedTiles)
        {
            OccupantType occupant = gameBoardManager.GetOccupantType(tile);
            if (occupant != OccupantType.None)
            {
                if (occupant == OccupantType.Player)
                {
                    hitPlayer = true;
                }
                else if (occupant == OccupantType.Enemy)
                {
                    hitEnemy = true;
                }

                Unit unit = gameBoardManager.GetUnitAtPosition(tile);
                PhysicalAttack(unit, 0);
            }
        }

        if (hitPlayer)
        {
            sfx.PlayUgh();
        }
        else if (hitEnemy)
        {
            sfx.PlayImpact();
        }
        else
        {
            sfx.PlaySwordWhoosh();
        }

        ClearHighlights();
        targetedTiles.Clear();
    }

    public void CancelAction()
    {
        ClearHighlights();
        targetedTiles.Clear();
    }

    protected void ClearHighlights()
    {
        foreach (Vector2Int tile in targetedTiles)
        {
            gameBoardManager.ClearHighlightAtPosition(tile, "Enemy");
        }
    }

    public override void TakeDamage(int damage)
    {
        CancelAction();
        base.TakeDamage(damage);
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void MoveTowards(Vector2Int targetPosition)
    {
        Vector2Int newPosition = FindClosestPosition(currentPosition, targetPosition, moveSpeed);
        Move(newPosition);
    }

    private Vector2Int FindClosestPosition(Vector2Int currentPosition, Vector2Int targetPosition, int moveSpeed)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();
        List<Vector2Int> directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        queue.Enqueue(currentPosition);
        distances[currentPosition] = 0;

        while (queue.Count > 0)
        {
            Vector2Int position = queue.Dequeue();
            int distance = distances[position];

            if (distance >= moveSpeed)
            {
                continue;
            }

            foreach (Vector2Int direction in directions)
            {
                Vector2Int newPosition = position + direction;

                if (gameBoardManager.IsPositionWithinBounds(newPosition) &&
                    !distances.ContainsKey(newPosition) &&
                    !gameBoardManager.IsPositionOccupied(newPosition))
                {
                    distances[newPosition] = distance + 1;
                    queue.Enqueue(newPosition);

                    if (newPosition == targetPosition)
                    {
                        return newPosition;
                    }
                }
            }
        }

        Vector2Int closestPosition = currentPosition;
        float closestDistance = Vector2Int.Distance(currentPosition, targetPosition);

        foreach (var kvp in distances)
        {
            float distanceToTarget = Vector2Int.Distance(kvp.Key, targetPosition);
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestPosition = kvp.Key;
            }
        }

        return closestPosition;
    }
}
