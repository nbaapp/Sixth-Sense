using UnityEngine;
using System.Collections.Generic;


public enum OccupantType
{
    None,
    Player,
    Enemy
}

public class OccupantInfo
{
    public Unit Unit;
    public OccupantType Type;

    public OccupantInfo(Unit unit, OccupantType type)
    {
        Unit = unit;
        Type = type;
    }
}
public class GameBoardManager : MonoBehaviour
{
    public GameObject plainsPrefab;
    public GameObject mountainsPrefab;
    public GameObject chasmPrefab;
    public GameObject waterPrefab;
    public GameObject moveHighlightPrefab;
    public GameObject attackHighlightPrefab;
    public GameObject enemyHighlightPrefab;
    private GridRenderer gridRenderer;
    public int gridSizeX = 10;
    public int gridSizeY = 10; 
    public Vector2Int gridSize = new Vector2Int(10, 10); // Define grid size

    private GameObject[,] tiles;
    private List<GameObject> moveHighlightObjects = new List<GameObject>(); // List to keep track of move highlight objects
    private List<GameObject> attackHighlightObjects = new List<GameObject>(); // List to keep track of attack highlight objects
    private List<GameObject> enemyHighlightObjects = new List<GameObject>(); // List to keep track of enemy highlight objects
    private Dictionary<Vector2Int, OccupantInfo> occupiedPositions = new Dictionary<Vector2Int, OccupantInfo>(); // Dictionary to track occupied positions

    void Start()
    {
        gridRenderer = FindObjectOfType<GridRenderer>();
        GenerateBoard();
    }

    void GenerateBoard()
    {
        gridSize = new Vector2Int(gridSizeX, gridSizeY);
        tiles = new GameObject[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject tile = Instantiate(plainsPrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.transform.SetParent(transform);
                tiles[x, y] = tile;
            }
        }
        gridRenderer.DrawGrid();
        OnBoardReady?.Invoke(); // Notify that the board is ready
    }

    public void HighlightTile(Vector2Int position, string highlightType)
    {
        if (IsPositionWithinBounds(position))
        {
            GameObject highlightPrefab = null;
            List<GameObject> highlightList = null;

            switch (highlightType)
            {
                case "Move":
                    highlightPrefab = moveHighlightPrefab;
                    highlightList = moveHighlightObjects;
                    break;
                case "Attack":
                    highlightPrefab = attackHighlightPrefab;
                    highlightList = attackHighlightObjects;
                    break;
                case "Enemy":
                    highlightPrefab = enemyHighlightPrefab;
                    highlightList = enemyHighlightObjects;
                    break;
            }

            if (highlightPrefab != null && highlightList != null)
            {
                GameObject highlight = Instantiate(highlightPrefab, new Vector3(position.x, position.y, -1), Quaternion.identity);
                highlightList.Add(highlight);
            }
        }
    }

    public void ClearHighlights(string highlightType)
    {
        List<GameObject> highlightList = null;

        switch (highlightType)
        {
            case "Move":
                highlightList = moveHighlightObjects;
                break;
            case "Attack":
                highlightList = attackHighlightObjects;
                break;
            case "Enemy":
                highlightList = enemyHighlightObjects;
                break;
        }

        if (highlightList != null)
        {
            foreach (GameObject highlight in highlightList)
            {
                Destroy(highlight);
            }
            highlightList.Clear();
        }
    }

    public void ClearHighlightAtPosition(Vector2Int position, string highlightType)
    {
        List<GameObject> highlightList = null;

        switch (highlightType)
        {
            case "Move":
                highlightList = moveHighlightObjects;
                break;
            case "Attack":
                highlightList = attackHighlightObjects;
                break;
            case "Enemy":
                highlightList = enemyHighlightObjects;
                break;
        }

        if (highlightList != null)
        {
            foreach (GameObject highlight in highlightList)
            {
                if (highlight.transform.position == new Vector3(position.x, position.y, -1))
                {
                    Destroy(highlight);
                    highlightList.Remove(highlight);
                    break;
                }
            }
        }
    }

    public bool IsPositionWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize.x && position.y >= 0 && position.y < gridSize.y;
    }

    public OccupantType GetOccupantType(Vector2Int position)
    {
        if (occupiedPositions.ContainsKey(position))
        {
            return occupiedPositions[position].Type;
        }
        return OccupantType.None;
    }

    public bool IsPositionOccupied(Vector2Int position, Vector2Int? ignorePosition = null)
    {
        if (ignorePosition.HasValue && position == ignorePosition.Value)
        {
            return false;
        }
        return occupiedPositions.ContainsKey(position);
    }

    public void SetUnitPosition(Vector2Int position, Unit unit)
    {
        if (IsPositionWithinBounds(position))
        {
            OccupantType type = unit is PlayerUnit ? OccupantType.Player : OccupantType.Enemy;
            occupiedPositions[position] = new OccupantInfo(unit, type);
        }
    }

    public void RemoveUnitPosition(Vector2Int position)
    {
        if (IsPositionWithinBounds(position))
        {
            occupiedPositions.Remove(position);
        }
    }

    public Unit GetUnitAtPosition(Vector2Int position)
    {
        if (IsPositionWithinBounds(position))
        {
            if (occupiedPositions.ContainsKey(position))
            {
                return occupiedPositions[position].Unit;
            }
        }
        return null;
    }

    public delegate void BoardReady();
    public event BoardReady OnBoardReady;

    
}
