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
    public bool gameRunning = true;
    private int turnCount = 0;

    public delegate void TurnEvent();
    public event TurnEvent OnPlayerTurnStart;
    public event TurnEvent OnPlayerTurnEnd;
    public event TurnEvent OnEnemyActionStart;
    public event TurnEvent OnEnemyActionEnd;
    private GameBoardManager gameBoardManager;
    private UIManager uiManager;

    public float CurrentTurnTime => currentTurnTime;
    public float RemainingTurnTime => turnTimer;
    bool feverMode = false;

    private void Awake()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        uiManager = FindObjectOfType<UIManager>();

        currentTurnTime = initialTurnTime;
        turnCount = 0;
        gameRunning = true;

        gameBoardManager.OnBoardReady += Begin;
    }
    
    private void Begin()
    {
        StartPlayerTurn();
    }

    private void Update()
    {
        TurnTimer();
    }

    private void TurnTimer() 
    {
        if (isPlayerTurn && gameRunning)
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

    private void StartPlayerTurn()
    {
        if (!gameRunning) return;
        isPlayerTurn = true;
        turnTimer = currentTurnTime;
        OnPlayerTurnStart?.Invoke(); // Notify that the player turn has started
    }

    public void EndPlayerTurn(bool resetTurnTime = false)
    {
        if (!gameRunning) return;
        if (resetTurnTime)
        {
            ResetTurnTimer();
            if (feverMode) {
                EndFeverMode();
            }
        }
        else
        {
            // Update the turn timer
            currentTurnTime = Mathf.Max(minTurnTime, currentTurnTime - turnTimeDecrease);
            if (currentTurnTime <= minTurnTime)
            {
                if (!feverMode) {
                    StartFeverMode();
                }
            }
        }

        turnCount++;
        isPlayerTurn = false;
        OnPlayerTurnEnd?.Invoke(); // Notify that the player turn has ended
        StartEnemyAction();
    }

    public void ResetTurnTimer()
    {
        if (currentTurnTime != initialTurnTime) {
            currentTurnTime = initialTurnTime;
        }
    }

    public void StartFeverMode()
    {
        if (!gameRunning) return;
        feverMode = true;
        uiManager.SetFeverModeSlider(feverMode);
        
    }

    public void EndFeverMode()
    {
        if (!gameRunning) return;
        feverMode = false;
        uiManager.SetFeverModeSlider(feverMode);
    }

    public bool IsFeverMode()
    {
        return feverMode;
    } 

    private void StartEnemyAction()
    {
        if (!gameRunning) return;
        OnEnemyActionStart?.Invoke(); // Notify that the enemy action has started
        StartCoroutine(EnemyAction());
    }

    private IEnumerator EnemyAction()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            yield return new WaitForSeconds(enemyTurnDuration);
            if (enemy != null)
            {
                enemy.SelectAction();
            }
        }

        OnEnemyActionEnd?.Invoke(); // Notify that the enemy action has ended
        StartPlayerTurn();
    }
}
