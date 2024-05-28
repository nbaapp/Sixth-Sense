using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject slimePrefab; // Add other enemy prefabs here as needed
    public int totalEnemiesToSpawn = 10;
    public int maxSpawnNumber = 3;
    public int spawnInterval = 5; // Number of turns between spawns
    public float slimeSpawnPercentage = 1.0f; // 100% chance to spawn Slime

    private GameBoardManager gameBoardManager;
    private TurnManager turnManager;
    private GameLogic gameLogic;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    void Start()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        turnManager = FindObjectOfType<TurnManager>();
        gameLogic = FindObjectOfType<GameLogic>();

        turnManager.OnPlayerTurnEnd += HandleTurnEnd;
        gameBoardManager.OnBoardReady += HandleBoardReady;
    }

    void HandleTurnEnd()
    {
        if (turnManager.GetTurnCount() % spawnInterval == 0 && enemiesSpawned < totalEnemiesToSpawn || enemiesAlive <= 0)
        {
            SpawnEnemies();
        }
    }

    void HandleBoardReady()
    {
        SpawnEnemies();
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            SpawnEnemies();
        }
    }

    private void WaveCompleted()
    {
        gameLogic.Victory();
    }

    private void SpawnEnemies()
    {
        int spawnCount = Random.Range(1, maxSpawnNumber + 1);
        for (int i = 0; i < spawnCount; i++)
        {
            if (enemiesSpawned >= totalEnemiesToSpawn && enemiesAlive <= 0) {
                WaveCompleted();
                return;
            }
            else if (enemiesSpawned < totalEnemiesToSpawn){
                Vector2Int spawnPosition = FindSpawnPosition();
                if (spawnPosition != Vector2Int.one * -1)
                {
                    float randomValue = Random.value;
                    if (randomValue <= slimeSpawnPercentage)
                    {
                        Instantiate(slimePrefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
                    }
                    // Add other enemy types spawn logic based on their percentage here
                    enemiesSpawned++;
                    enemiesAlive++;
                }
            } 
        }
    }

    Vector2Int FindSpawnPosition()
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        for (int x = 0; x < gameBoardManager.gridSize.x; x++)
        {
            for (int y = 0; y < gameBoardManager.gridSize.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (!gameBoardManager.IsPositionOccupied(position))
                {
                    availablePositions.Add(position);
                }
            }
        }

        if (availablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            return availablePositions[randomIndex];
        }

        return Vector2Int.one * -1; // No available position found
    }
}
