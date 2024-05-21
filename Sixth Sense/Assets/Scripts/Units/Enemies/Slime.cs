using UnityEngine;
using System.Collections.Generic;

public class Slime : Enemy
{
    private Vector2Int targetPosition;
    private List<Vector2Int> attackPositions = new List<Vector2Int>();

    public GameObject enemyAttackHighlightPrefab;

    public override void SelectAction()
    {
        Vector2Int playerPosition = Vector2Int.RoundToInt(playerUnit.transform.position);
        Vector2Int slimePosition = Vector2Int.RoundToInt(transform.position);

        targetPosition = FindNearestAdjacentPosition(slimePosition, playerPosition);

        if (targetPosition != slimePosition)
        {
            attackPositions.Clear();
            attackPositions.Add(targetPosition);
            HighlightAttackPositions();
        }
    }

    public override void ExecuteAction()
    {
        foreach (Vector2Int pos in attackPositions)
        {
            if (pos == Vector2Int.RoundToInt(playerUnit.transform.position))
            {
                playerUnit.TakeDamage(strength);
            }
        }
        ClearHighlights();
    }

    protected override void ClearHighlights()
    {
        foreach (var pos in attackPositions)
        {
            gameBoardManager.ClearHighlightAtPosition(pos, enemyAttackHighlightPrefab);
        }
        attackPositions.Clear();
    }

    private Vector2Int FindNearestAdjacentPosition(Vector2Int slimePosition, Vector2Int playerPosition)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        float minDistance = float.MaxValue;
        Vector2Int bestPosition = slimePosition;

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = playerPosition + dir;
            if (gameBoardManager.IsPositionWithinBounds(newPos) && !gameBoardManager.IsPositionOccupied(newPos))
            {
                float distance = Vector2Int.Distance(newPos, slimePosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestPosition = newPos;
                }
            }
        }
        return bestPosition;
    }

    private void HighlightAttackPositions()
    {
        foreach (var pos in attackPositions)
        {
            gameBoardManager.HighlightTile(pos, enemyAttackHighlightPrefab);
        }
    }
}
