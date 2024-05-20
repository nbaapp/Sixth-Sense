using UnityEngine;
using System.Collections.Generic;

public class GameBoardManager : MonoBehaviour
{
    public GameObject plainsPrefab;
    public GameObject mountainsPrefab;
    public GameObject chasmPrefab;
    public GameObject waterPrefab;
    public GameObject highlightPrefab; // Prefab for the highlight overlay
    public Vector2Int gridSize = new Vector2Int(10, 10); // Define grid size

    private GameObject[,] tiles;
    private List<GameObject> highlightObjects = new List<GameObject>(); // List to keep track of highlight objects

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
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
        OnBoardReady?.Invoke(); // Notify that the board is ready
    }

    public void HighlightTile(Vector2Int position)
    {
        if (position.x >= 0 && position.x < gridSize.x && position.y >= 0 && position.y < gridSize.y)
        {
            GameObject highlight = Instantiate(highlightPrefab, new Vector3(position.x, position.y, -1), Quaternion.identity); // Ensure the overlay is above the tile
            highlight.transform.SetParent(transform);
            highlightObjects.Add(highlight);
        }
    }

    public void ClearHighlights()
    {
        foreach (GameObject highlight in highlightObjects)
        {
            Destroy(highlight);
        }
        highlightObjects.Clear();
    }

    public delegate void BoardReady();
    public event BoardReady OnBoardReady;
}
