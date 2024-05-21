using UnityEngine;
using System.Collections.Generic;

public class PlayerUnit : Unit
{
    public Weapon equippedWeapon;
    public Special equippedSpecial;
    private int specialCooldown;

    private List<GameObject> actionHighlights = new List<GameObject>();

    public int SpecialCooldown => specialCooldown; // Getter for the special cooldown

    public void Attack(Vector2Int position, Vector2Int direction, GameObject highlightPrefab)
    {
        if (equippedWeapon == null) return;

        ClearActionHighlights();

        List<Vector2Int> attackTiles = equippedWeapon.GetAttackTiles(position, direction);

        foreach (var tile in attackTiles)
        {
            GameObject highlight = Instantiate(highlightPrefab, new Vector3(tile.x, tile.y, -1), Quaternion.identity);
            actionHighlights.Add(highlight);
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

        // Clear action highlights
        ClearActionHighlights();
    }

    public void Special(Vector2Int position, Vector2Int direction, GameObject highlightPrefab)
{
    if (equippedSpecial == null || specialCooldown > 0) return;

    ClearActionHighlights();

    List<Vector2Int> specialTiles = equippedSpecial.GetAffectedTiles(position, direction);

    foreach (var tile in specialTiles)
    {
        GameObject highlight = Instantiate(highlightPrefab, new Vector3(tile.x, tile.y, -1), Quaternion.identity);
        actionHighlights.Add(highlight);
    }
}

public void ExecuteSpecial(Vector2Int position, Vector2Int direction)
{
    if (equippedSpecial == null || specialCooldown > 0) return;

    equippedSpecial.ExecuteSpecial(position, direction);
    specialCooldown = equippedSpecial.cooldown;

    // Clear action highlights
    ClearActionHighlights();
}


    public void ClearActionHighlights()
    {
        foreach (var highlight in actionHighlights)
        {
            Destroy(highlight);
        }
        actionHighlights.Clear();
    }

    public void ReduceSpecialCooldown()
    {
        if (specialCooldown > 0)
        {
            specialCooldown--;
        }
    }

    public void Block()
    {
        // Placeholder for block action
        Debug.Log("Player Block");
    }

    protected override void Die()
    {
        base.Die();
        // Additional player-specific death handling
    }
}
