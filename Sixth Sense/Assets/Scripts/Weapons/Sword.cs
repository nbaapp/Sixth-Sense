using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Sword", menuName = "Weapons/Sword", order = 1)]
public class Sword : Weapon
{
    public override List<Vector2Int> GetAttackTiles(Vector2Int position, Vector2Int direction)
    {
        List<Vector2Int> attackTiles = new List<Vector2Int>();

        if (direction == Vector2Int.up)
        {
            attackTiles.Add(position + Vector2Int.up);
            attackTiles.Add(position + Vector2Int.up + Vector2Int.left);
            attackTiles.Add(position + Vector2Int.up + Vector2Int.right);
        }
        else if (direction == Vector2Int.right)
        {
            attackTiles.Add(position + Vector2Int.right);
            attackTiles.Add(position + Vector2Int.right + Vector2Int.up);
            attackTiles.Add(position + Vector2Int.right + Vector2Int.down);
        }
        else if (direction == Vector2Int.down)
        {
            attackTiles.Add(position + Vector2Int.down);
            attackTiles.Add(position + Vector2Int.down + Vector2Int.left);
            attackTiles.Add(position + Vector2Int.down + Vector2Int.right);
        }
        else if (direction == Vector2Int.left)
        {
            attackTiles.Add(position + Vector2Int.left);
            attackTiles.Add(position + Vector2Int.left + Vector2Int.up);
            attackTiles.Add(position + Vector2Int.left + Vector2Int.down);
        }

        return attackTiles;
    }
}
