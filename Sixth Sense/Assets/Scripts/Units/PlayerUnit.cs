using UnityEngine;
using System.Collections.Generic;

public class PlayerUnit : Unit
{
    public Weapon equippedWeapon;

    private List<GameObject> attackHighlights = new List<GameObject>();

    public void Attack(Vector2Int position, Vector2Int direction, GameObject highlightPrefab)
    {
        if (equippedWeapon == null) return;

        List<Vector2Int> attackTiles = equippedWeapon.GetAttackTiles(position, direction);

        foreach (var tile in attackTiles)
        {
            GameObject highlight = Instantiate(highlightPrefab, new Vector3(tile.x, tile.y, -1), Quaternion.identity);
            attackHighlights.Add(highlight);
        }
    }

    public void ExecuteAttack(Vector2Int position, Vector2Int direction)
    {
        if (equippedWeapon == null) return;

        List<Vector2Int> attackTiles = equippedWeapon.GetAttackTiles(position, direction);

        foreach (var tile in attackTiles)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(tile.x, tile.y));
            foreach (var collider in colliders)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit != null && unit != this)
                {
                    unit.TakeDamage(strength); // Assume physical attack, using strength
                }
            }
        }

        // Clear attack highlights
        ClearAttackHighlights();
    }

    public void ClearAttackHighlights()
    {
        foreach (var highlight in attackHighlights)
        {
            Destroy(highlight);
        }
        attackHighlights.Clear();
    }

    public void Block()
    {
        // Placeholder for block action
        Debug.Log("Player Block");
    }

    public void Special()
    {
        // Placeholder for special action
        Debug.Log("Player Special");
    }

    protected override void Die()
    {
        base.Die();
        // Additional player-specific death handling
    }
}
