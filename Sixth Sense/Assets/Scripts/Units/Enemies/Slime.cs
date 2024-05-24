using UnityEngine;
using System.Collections.Generic;

public class Slime : Enemy
{
    private Vector2Int targetPosition;
    private Vector2Int attackPosition;
    public int TacklePower = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SelectAction()
    {
        Tackle();
    }

    private void Tackle()
    {
        Vector2Int playerPosition = playerUnit.GetUnitPosition();
        Vector2Int currentPosition = GetUnitPosition();

        // Determine if adjacent to player
        List<Vector2Int> adjacentPositions = GetAdjacentPositions(currentPosition);
        bool isAdjacentToPlayer = adjacentPositions.Contains(playerPosition);

        if (isAdjacentToPlayer)
        {
            // If adjacent to the player, attack the player's position
            attackPosition = playerPosition;
            targetedTiles.Add(attackPosition);
            gameBoardManager.HighlightTile(attackPosition, "Enemy");
        }
        else
        {
            // Move towards the player within movement range
            targetPosition = GetPositionWithinMovementRange(currentPosition, playerPosition, moveSpeed);
            Move(targetPosition);

            // After moving, determine the new adjacent positions
            adjacentPositions = GetAdjacentPositions(targetPosition);
            isAdjacentToPlayer = adjacentPositions.Contains(playerPosition);

            if (isAdjacentToPlayer)
            {
                // If now adjacent to the player, attack the player's position
                attackPosition = playerPosition;
            }
            else
            {
                // Otherwise, select a random adjacent position to attack
                attackPosition = SelectRandomAttackPosition(adjacentPositions);
            }

            targetedTiles.Add(attackPosition);
            gameBoardManager.HighlightTile(attackPosition, "Enemy");
        }
    }

    private List<Vector2Int> GetAdjacentPositions(Vector2Int position)
    {
        List<Vector2Int> adjacentPositions = new List<Vector2Int>
        {
            position + Vector2Int.up,
            position + Vector2Int.down,
            position + Vector2Int.left,
            position + Vector2Int.right
        };

        return adjacentPositions;
    }

    private Vector2Int SelectRandomAttackPosition(List<Vector2Int> adjacentPositions)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        foreach (Vector2Int pos in adjacentPositions)
        {
            if (!gameBoardManager.IsPositionOccupied(pos) || gameBoardManager.GetOccupantType(pos) == OccupantType.Player)
            {
                validPositions.Add(pos);
            }
        }

        if (validPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validPositions.Count);
            return validPositions[randomIndex];
        }

        return GetUnitPosition(); // No valid position to attack
    }
}
