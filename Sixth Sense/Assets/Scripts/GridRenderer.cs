using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public float cellSize = 1f;
    public Color lineColor = Color.black;
    private GameBoardManager gameBoardManager;

    private void Start()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
    }

    public void DrawGrid()
    {
        Vector2Int gridSize = gameBoardManager.gridSize;

        for (int i = 0; i <= gridSize.y; i++)
        {
            CreateLine(new Vector3(-0.5f, i * cellSize - 0.5f, 0), new Vector3(gridSize.x * cellSize - 0.5f, i * cellSize - 0.5f, 0));
        }

        for (int j = 0; j <= gridSize.x; j++)
        {
            CreateLine(new Vector3(j * cellSize - 0.5f, -0.5f, 0), new Vector3(j * cellSize - 0.5f, gridSize.y * cellSize - 0.5f, 0));
        }
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(transform);
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lineColor;
        lr.endColor = lineColor;
    }
}
