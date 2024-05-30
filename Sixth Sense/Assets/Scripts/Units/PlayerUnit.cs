using UnityEngine;
using System.Collections.Generic;

public class PlayerUnit : Unit
{
    public Weapon equippedWeapon;
    public Special equippedSpecial;
    private int specialCooldown;
    public int SpecialCooldown => specialCooldown; // Getter for the special cooldown
    private PlayerController playerController;
    private SFX sfx;
    private GameLogic gameLogic;
    public HealthBar healthBar;
    private bool hitEnemy = false;

    protected override void Start()
    {
        base.Start();
        playerController = FindObjectOfType<PlayerController>();
        sfx = FindObjectOfType<SFX>();
        gameLogic = FindObjectOfType<GameLogic>();
        gameBoardManager = FindObjectOfType<GameBoardManager>(); // Get the GameBoardManager instance

        currentPosition = Vector2Int.RoundToInt(transform.position);
        gameBoardManager.SetUnitPosition(currentPosition, this); // Register the initial position
        healthBar.SetMaxHealth(maxHealth);
    }

    public override void Move(Vector2Int newPosition) {
        playerController.DisableControls();

        gameBoardManager.RemoveUnitPosition(currentPosition);
        StartCoroutine(LerpPosition(newPosition, moveDuration));
        currentPosition = newPosition;
        gameBoardManager.SetUnitPosition(newPosition, this);

        playerController.EnableControls();
    }

    public void Attack(Vector2Int position, Vector2Int direction)
    {
        if (equippedWeapon == null) return;

        gameBoardManager.ClearHighlights("Attack");

        List<Vector2Int> attackTiles = equippedWeapon.GetAttackTiles(position, direction);

        foreach (var tile in attackTiles)
        {
            gameBoardManager.HighlightTile(tile, "Attack");
        }
    }

    public void ExecuteAttack(Vector2Int position, Vector2Int direction)
    {
        if (equippedWeapon == null) return;

        hitEnemy = false;

        List<Vector2Int> attackTiles = equippedWeapon.GetAttackTiles(position, direction);

        foreach (var tile in attackTiles)
        {
            if (gameBoardManager.GetOccupantType(tile) != OccupantType.None)
            {
                hitEnemy = true;
                Unit unit = gameBoardManager.GetUnitAtPosition(tile);
                if (equippedWeapon.isMagical)
                {
                    MagicalAttack(unit, equippedWeapon.magicalStrength);
                }
                else if (equippedWeapon.isPhysical)
                {
                    PhysicalAttack(unit, equippedWeapon.physicalStrength);
                }
            }
        }

        if (hitEnemy)
        {
            sfx.PlayImpact();
        }
        else
        {
            sfx.PlaySwordWhoosh();
        }

        // Clear attack highlights
        gameBoardManager.ClearHighlights("Attack");
    }

    public void Special(Vector2Int position, Vector2Int direction)
    {
        if (equippedSpecial == null || specialCooldown > 0) return;

        gameBoardManager.ClearHighlights("Attack"); // Assuming same highlight type for special

        List<Vector2Int> specialTiles = equippedSpecial.GetAffectedTiles(position, direction);

        foreach (var tile in specialTiles)
        {
            gameBoardManager.HighlightTile(tile, "Attack"); // Assuming same highlight type for special
        }
    }

    public void ExecuteSpecial(Vector2Int position, Vector2Int direction)
    {
        if (equippedSpecial == null || specialCooldown > 0) return;

        equippedSpecial.ExecuteSpecial(position, direction);
        specialCooldown = equippedSpecial.cooldown;

        ScreenShake.Instance.TriggerShake(0.2f, 0.2f);

        // Clear special highlights
        gameBoardManager.ClearHighlights("Attack"); // Assuming same highlight type for special
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
        gameBoardManager.RemoveUnitPosition(currentPosition);
        gameLogic.GameOver();
        base.Die();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        healthBar.SetHealth(currentHealth);
        ScreenShake.Instance.TriggerShake(0.2f, 0.1f);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        healthBar.SetHealth(currentHealth);
    }
}
