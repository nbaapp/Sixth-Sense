using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Earthquake", menuName = "Specials/Earthquake", order = 1)]
public class Earthquake : Special
{
    public override List<Vector2Int> GetAffectedTiles(Vector2Int position, Vector2Int direction)
    {
        List<Vector2Int> affectedTiles = new List<Vector2Int>();

        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= 2)
                {
                    Vector2Int tile = position + new Vector2Int(x, y);
                    if (tile != position) // Exclude the player's own position
                    {
                        affectedTiles.Add(tile);
                    }
                }
            }
        }

        return affectedTiles;
    }

    public override void ExecuteSpecial(Vector2Int position, Vector2Int direction, int damage)
    {
        if (playerUnit == null) getPlayerUnit();
        if (gameBoardManager == null) getGameBoardManager();
        
        List<Vector2Int> affectedTiles = GetAffectedTiles(position, direction);

        foreach (var tile in affectedTiles)
        {
            if (gameBoardManager.GetOccupantType(tile) == OccupantType.Enemy)
            {
                Unit unit = gameBoardManager.GetUnitAtPosition(tile);
                if (isMagical)
                {
                    playerUnit.MagicalAttack(unit, damage);
                }
                else if (isPhysical)
                {
                    playerUnit.PhysicalAttack(unit, damage);
                }
            }
        }
    }
}
