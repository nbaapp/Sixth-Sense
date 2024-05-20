using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public float initialTurnTime = 15f; // Initial turn time
    public float minTurnTime = 2f; // Minimum turn time
    public float turnTimeDecrease = 1f; // Amount to decrease turn time after each successful action
    public float enemyTurnDuration = 1f; // Duration of the enemy turn

    private float currentTurnTime;
    private float turnTimer;
    public bool isPlayerTurn { get; private set; } = true; // Expose isPlayerTurn as a public property

    public delegate void TurnEvent();
    public event TurnEvent OnPlayerTurnStart;
    public event TurnEvent OnPlayerTurnEnd;
    public event TurnEvent OnEnemyTurnStart;
    public event TurnEvent OnEnemyTurnEnd;

    public float CurrentTurnTime => currentTurnTime;
    public float RemainingTurnTime => turnTimer;

    private void Start()
    {
        currentTurnTime = initialTurnTime;
        StartPlayerTurn();
    }

    private void Update()
    {
        if (isPlayerTurn)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0)
            {
                EndPlayerTurn(resetTurnTime: true);
            }
        }
    }

    private void StartPlayerTurn()
    {
        isPlayerTurn = true;
        turnTimer = currentTurnTime;
        OnPlayerTurnStart?.Invoke(); // Notify that the player turn has started
    }

    public void EndPlayerTurn(bool resetTurnTime = false)
    {
        if (resetTurnTime)
        {
            currentTurnTime = initialTurnTime;
        }
        else
        {
            // Update the turn timer
            currentTurnTime = Mathf.Max(minTurnTime, currentTurnTime - turnTimeDecrease);
        }
        
        isPlayerTurn = false;
        OnPlayerTurnEnd?.Invoke(); // Notify that the player turn has ended
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        OnEnemyTurnStart?.Invoke(); // Notify that the enemy turn has started

        // Pause for the duration of the enemy turn
        yield return new WaitForSeconds(enemyTurnDuration);

        OnEnemyTurnEnd?.Invoke(); // Notify that the enemy turn has ended

        // Start the next player turn
        StartPlayerTurn();
    }
}
