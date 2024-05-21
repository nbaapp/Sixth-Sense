using UnityEngine;

public abstract class Enemy : Unit
{
    protected GameBoardManager gameBoardManager;
    protected PlayerUnit playerUnit;
    protected TurnManager turnManager;

    protected virtual void Awake()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        playerUnit = FindObjectOfType<PlayerUnit>();
        turnManager = FindObjectOfType<TurnManager>();
    }

    public abstract void SelectAction();

    public abstract void ExecuteAction();

    public void CancelAction()
    {
        ClearHighlights();
    }

    protected abstract void ClearHighlights();

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHealth > 0)
        {
            CancelAction();
        }
    }
}
