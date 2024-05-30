using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements.Experimental;

public class Slime : Enemy
{
    private Vector2Int targetPosition;
    private Vector2Int attackPosition;
    public int TacklePower = 0;
    private bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        isAttacking = false;
    }

    public override void SelectAction()
    {
        if (isAttacking) {
            ExecuteAction();

            if (gameBoardManager.GetOccupantType(attackPosition) == OccupantType.None)
            {
                Move(attackPosition);
            }

            isAttacking = false;
        }
        else if (IsAdjacentToPlayer()) {
            Tackle();
            isAttacking = true;
        }
        else {
            MoveTowards(playerUnit.GetUnitPosition());
        }
    }

    private void Tackle()
    {
        Vector2Int playerPosition = playerUnit.GetUnitPosition();
        
        attackPosition = playerPosition;
        targetedTiles.Add(attackPosition);
        gameBoardManager.HighlightTile(attackPosition, "Enemy");
    }

    private bool IsAdjacentToPlayer()
    {
        Vector2Int playerPosition = playerUnit.GetUnitPosition();
        Vector2Int currentPosition = GetUnitPosition();

        List<Vector2Int> adjacentPositions = GetAdjacentPositions(currentPosition);
        return adjacentPositions.Contains(playerPosition);
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

        return GetUnitPosition();
    }
}
