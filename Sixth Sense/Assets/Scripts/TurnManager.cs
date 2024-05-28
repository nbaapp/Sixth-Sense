using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public float initialTurnTime = 15f; // Initial turn time
    public float minTurnTime = 2f; // Minimum turn time
    public float turnTimeDecrease = 1f; // Amount to decrease turn time after each successful action
    public float enemyTurnDuration = 1f; // Duration of the enemy action execution

    private float currentTurnTime;
    private float turnTimer;
    public bool isPlayerTurn { get; private set; } = false; // Expose isPlayerTurn as a public property
    private int turnCount = 0;

    public delegate void TurnEvent();
    public event TurnEvent OnPlayerTurnStart;
    public event TurnEvent OnPlayerTurnEnd;
    public event TurnEvent OnEnemyActionSelectStart;
    public event TurnEvent OnEnemyActionSelectEnd;
    public event TurnEvent OnEnemyActionExecuteStart;
    public event TurnEvent OnEnemyActionExecuteEnd;
    private GameBoardManager gameBoardManager;

    public float CurrentTurnTime => currentTurnTime;
    public float RemainingTurnTime => turnTimer;

    private void Start()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();

        currentTurnTime = initialTurnTime;
        turnCount = 0;

        StartEnemyActionSelect();
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

    public int GetTurnCount()
    {
        return turnCount;
    }

    private void StartEnemyActionSelect()
    {
        OnEnemyActionSelectStart?.Invoke(); // Notify that the enemy action selection has started
        StartCoroutine(EnemyActionSelect());
    }

    private IEnumerator EnemyActionSelect()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy != null)
            {
                enemy.SelectAction();
                yield return new WaitForSeconds(enemyTurnDuration);
            }
        }

        OnEnemyActionSelectEnd?.Invoke(); // Notify that the enemy action selection has ended

        StartPlayerTurn();
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
        
        turnCount++;
        isPlayerTurn = false;
        OnPlayerTurnEnd?.Invoke(); // Notify that the player turn has ended
        StartCoroutine(EnemyActionExecute());
    }

    private IEnumerator EnemyActionExecute()
    {
        OnEnemyActionExecuteStart?.Invoke(); // Notify that the enemy action execution has started

        // Simulate enemy action execution duration
        yield return new WaitForSeconds(enemyTurnDuration);

        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy != null)
            {
                enemy.ExecuteAction();
            }
        }

        gameBoardManager.ClearHighlights("Enemy");

        OnEnemyActionExecuteEnd?.Invoke(); // Notify that the enemy action execution has ended

        // Start the next enemy action selection
        StartEnemyActionSelect();
    }
}
