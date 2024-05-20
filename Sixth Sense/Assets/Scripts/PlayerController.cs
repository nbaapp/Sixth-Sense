using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private PlayerUnit playerUnit;
    private Vector2Int gridPosition; // Current grid position
    private Vector2Int initialPosition; // Initial position for the turn
    private PlayerInputActions playerInputActions;
    private GameBoardManager gameBoardManager;
    private TurnManager turnManager;
    private bool isMoving;
    private float moveTimer;
    public float moveTime = 0.2f; // Time between moves
    private List<Vector2Int> reachableTiles;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerUnit = GetComponent<PlayerUnit>();
        gridPosition = Vector2Int.RoundToInt(transform.position);
    }

    private void Start()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        turnManager = FindObjectOfType<TurnManager>();
        initialPosition = gridPosition;
        gameBoardManager.OnBoardReady += HighlightReachableTiles; // Subscribe to the event

        turnManager.OnPlayerTurnStart += StartPlayerTurn;
        turnManager.OnPlayerTurnEnd += EndPlayerTurn;
    }

    private void OnEnable()
    {
        playerInputActions.Player.ConfirmMove.performed += OnConfirmMove;
    }

    private void OnDisable()
    {
        playerInputActions.Player.ConfirmMove.performed -= OnConfirmMove;
        gameBoardManager.OnBoardReady -= HighlightReachableTiles; // Unsubscribe from the event

        if (turnManager != null)
        {
            turnManager.OnPlayerTurnStart -= StartPlayerTurn;
            turnManager.OnPlayerTurnEnd -= EndPlayerTurn;
        }
    }

    private void Update()
    {
        if (turnManager != null && turnManager.isPlayerTurn)
        {
            if (!isMoving)
            {
                moveTimer -= Time.deltaTime;
                if (moveTimer <= 0)
                {
                    Move();
                }
            }
        }
    }

    private void Move()
    {
        Vector2 moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (moveInput == Vector2.zero)
        {
            return; // No input, do not move
        }

        Vector2Int targetGridPosition = gridPosition;

        if (moveInput.x > 0) targetGridPosition += Vector2Int.right;
        else if (moveInput.x < 0) targetGridPosition += Vector2Int.left;
        else if (moveInput.y > 0) targetGridPosition += Vector2Int.up;
        else if (moveInput.y < 0) targetGridPosition += Vector2Int.down;

        targetGridPosition.x = Mathf.Clamp(targetGridPosition.x, 0, gameBoardManager.gridSize.x - 1);
        targetGridPosition.y = Mathf.Clamp(targetGridPosition.y, 0, gameBoardManager.gridSize.y - 1);

        if (reachableTiles.Contains(targetGridPosition))
        {
            MoveToGridPosition(targetGridPosition);
            moveTimer = moveTime; // Set a fixed cooldown time between moves
        }
    }

    private void MoveToGridPosition(Vector2Int targetGridPosition)
    {
        Vector3 targetPosition = new Vector3(targetGridPosition.x, targetGridPosition.y, transform.position.z);
        transform.position = targetPosition;
        gridPosition = targetGridPosition;
        isMoving = false; // Allow the next movement
    }

    private void HighlightReachableTiles()
    {
        // Clear previous highlights
        gameBoardManager.ClearHighlights();

        reachableTiles = new List<Vector2Int>();
        for (int x = -playerUnit.moveSpeed; x <= playerUnit.moveSpeed; x++)
        {
            for (int y = -playerUnit.moveSpeed; y <= playerUnit.moveSpeed; y++)
            {
                Vector2Int tilePosition = initialPosition + new Vector2Int(x, y);
                if (Mathf.Abs(x) + Mathf.Abs(y) <= playerUnit.moveSpeed && 
                    tilePosition.x >= 0 && tilePosition.x < gameBoardManager.gridSize.x &&
                    tilePosition.y >= 0 && tilePosition.y < gameBoardManager.gridSize.y)
                {
                    reachableTiles.Add(tilePosition);
                }
            }
        }

        // Highlight tiles with overlay objects
        foreach (Vector2Int tile in reachableTiles)
        {
            gameBoardManager.HighlightTile(tile);
        }
    }

    private void OnConfirmMove(InputAction.CallbackContext context)
    {
        initialPosition = gridPosition; // Update initial position after confirming move
        HighlightReachableTiles(); // Highlight reachable tiles for the new position
        turnManager.EndPlayerTurn(resetTurnTime: false); // End player turn after confirming move
    }

    private void StartPlayerTurn()
    {
        HighlightReachableTiles(); // Highlight reachable tiles at the start of the turn
    }

    private void EndPlayerTurn()
    {
        // Update initial position even if no move is made
        initialPosition = gridPosition;
        // Clear highlights at the end of the turn
        gameBoardManager.ClearHighlights();
    }
}
