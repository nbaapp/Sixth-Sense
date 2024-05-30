using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private PlayerUnit playerUnit;
    private Vector2Int gridPosition;
    private Vector2Int initialPosition;
    private PlayerInputActions playerInputActions;
    private GameBoardManager gameBoardManager;
    private TurnManager turnManager;

    private bool isMoving;
    private float moveTimer;
    public float moveTime = 2f;
    private List<Vector2Int> reachableTiles = new List<Vector2Int>();
    public TextMeshProUGUI specialIndicator;
    private bool isAttacking;
    private bool isSpecialing;
    private Vector2Int attackDirection = Vector2Int.up;
    private bool isLockingPosition;

    private void Awake()
    {
        playerUnit = GetComponent<PlayerUnit>();
        playerInputActions = new PlayerInputActions();
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        turnManager = FindObjectOfType<TurnManager>();

        initialPosition = Vector2Int.RoundToInt(transform.position);
        gridPosition = initialPosition;
        gameBoardManager.SetUnitPosition(gridPosition, playerUnit);

        playerInputActions.Player.Attack.performed += ctx => OnAttack();
        playerInputActions.Player.Special.performed += ctx => OnSpecial();
        playerInputActions.Player.Block.performed += ctx => OnBlock();
        playerInputActions.Player.LockPosition.performed += ctx => OnLockPosition();
        playerInputActions.Player.LockPosition.canceled += ctx => OnLockPositionCanceled();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        turnManager.OnPlayerTurnStart += StartPlayerTurn;
        turnManager.OnPlayerTurnEnd += EndPlayerTurn;
        gameBoardManager.OnBoardReady += Begin;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
        turnManager.OnPlayerTurnStart -= StartPlayerTurn;
        turnManager.OnPlayerTurnEnd -= EndPlayerTurn;
    }

    private void Begin()
    {
        UpdateSpecialIndicator();
    }

    private void Update()
    {
        if (!turnManager.isPlayerTurn || isMoving) return;

        Vector2 inputDirection = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (inputDirection != Vector2.zero)
        {
            MoveOrRotate(Vector2Int.RoundToInt(inputDirection));
        }
    }

    private void MoveOrRotate(Vector2Int direction)
    {
        if (isLockingPosition)
        {
            Rotate(direction);
        }
        else
        {
            Move(gridPosition + direction);
            Rotate(direction);
        }
    }

    private void Move(Vector2Int targetPosition)
    {
        if (!reachableTiles.Contains(targetPosition)) return;
        ClearActionState();
        Debug.Log($"Moving to {targetPosition}");
        playerUnit.Move(targetPosition); // Use Unit's Move method
        gridPosition = targetPosition;
        isMoving = true;
        moveTimer = moveTime;


        Invoke(nameof(StopMoving), moveTimer);

        turnManager.EndPlayerTurn(resetTurnTime: false); // End turn after moving
    }

    private void StopMoving()
    {
        isMoving = false;
    }

    private void Rotate(Vector2Int direction)
    {
        if (direction.x > 0) attackDirection = Vector2Int.right;
        else if (direction.x < 0) attackDirection = Vector2Int.left;
        else if (direction.y > 0) attackDirection = Vector2Int.up;
        else if (direction.y < 0) attackDirection = Vector2Int.down;

        
        gameBoardManager.ClearHighlights("Attack");
        playerUnit.Attack(gridPosition, attackDirection);
        
    }

    private void HighlightReachableTiles()
    {
        gameBoardManager.ClearHighlights("Move");
        reachableTiles.Clear();

        Debug.Log("Highlighting reachable tiles");

        // Calculate reachable tiles based on the player's move speed
        for (int x = -playerUnit.moveSpeed; x <= playerUnit.moveSpeed; x++)
        {
            for (int y = -playerUnit.moveSpeed; y <= playerUnit.moveSpeed; y++)
            {
                Vector2Int newPos = gridPosition + new Vector2Int(x, y);
                if (Mathf.Abs(x) + Mathf.Abs(y) <= playerUnit.moveSpeed &&
                    gameBoardManager.IsPositionWithinBounds(newPos) && !gameBoardManager.IsPositionOccupied(newPos, gridPosition))
                {
                    reachableTiles.Add(newPos);
                }
            }
        }

        // Highlight tiles with overlay objects
        foreach (Vector2Int tile in reachableTiles)
        {
            gameBoardManager.HighlightTile(tile, "Move");
        }
    }

    private void OnAttack()
    {
        /*
        if (isAttacking)
        {
            playerUnit.ExecuteAttack(gridPosition, attackDirection);
            isAttacking = false;
            turnManager.EndPlayerTurn(resetTurnTime: false); // End turn after executing attack
        }
        else
        {
            playerUnit.Attack(gridPosition, attackDirection);
            isAttacking = true;
            isSpecialing = false; // Cancel special if it's in progress
        }
        */
        playerUnit.ExecuteAttack(gridPosition, attackDirection);
        isAttacking = false;
        turnManager.EndPlayerTurn(resetTurnTime: false); // End turn after executing attack
    }

    private void OnSpecial()
    {
        /*
        if (isSpecialing)
        {
            playerUnit.ExecuteSpecial(gridPosition, attackDirection);
            isSpecialing = false;
            turnManager.EndPlayerTurn(resetTurnTime: false); // End turn after executing special
            UpdateSpecialIndicator();
        }
        else if (playerUnit.SpecialCooldown == 0)
        {
            playerUnit.Special(gridPosition, attackDirection);
            isSpecialing = true;
            isAttacking = false; // Cancel attack if it's in progress
        }
        */
        playerUnit.ExecuteSpecial(gridPosition, attackDirection);
        turnManager.EndPlayerTurn(resetTurnTime: false); // End turn after executing special
        UpdateSpecialIndicator();
    }

    private void OnBlock()
    {
        PerformAction(playerUnit.Block);
    }

    private void PerformAction(System.Action action)
    {
        ClearActionState();
        action.Invoke();
        initialPosition = gridPosition; // Update initial position after performing action
        HighlightReachableTiles(); // Highlight reachable tiles for the new position
        turnManager.EndPlayerTurn(resetTurnTime: false); // End player turn after performing action
    }


    private void ClearActionState()
    {
        if (isAttacking || isSpecialing)
        {
            gameBoardManager.ClearHighlights("Attack");
            isAttacking = false;
            isSpecialing = false;
        }
    }

    private void OnLockPosition()
    {
        isLockingPosition = true;
    }

    private void OnLockPositionCanceled()
    {
        isLockingPosition = false;
    }

    private void StartPlayerTurn()
    {
        playerUnit.ReduceSpecialCooldown();
        UpdateSpecialIndicator();
        HighlightReachableTiles(); // Highlight reachable tiles at the start of the turn
        Rotate(attackDirection); // set attack direction
    }

    private void EndPlayerTurn()
    {
        // Update initial position even if no move is made
        initialPosition = gridPosition;
        // Clear highlights at the end of the turn
        gameBoardManager.ClearHighlights("Move");
        ClearActionState(); // Clear action highlights if any
        isLockingPosition = false;
    }

    private void UpdateSpecialIndicator()
    {
        if (playerUnit.SpecialCooldown > 0)
        {
            specialIndicator.text = "Special";
            specialIndicator.color = Color.red;
        }
        else
        {
            specialIndicator.text = "Special";
            specialIndicator.color = Color.green;
        }
    }

    public void DisableControls()
    {
        playerInputActions.Player.Disable();
    }

    public void EnableControls()
    {
        playerInputActions.Player.Enable();
    }
}
