using UnityEngine;
using System.Collections.Generic;

public abstract class Special : ScriptableObject
{
    public string specialName;
    public int cooldown;
    public int physicalStrength = 0;
    public int magicalStrength = 1;
    public bool isPhysical = false;
    public bool isMagical = true;
    public GameBoardManager gameBoardManager;
    public PlayerUnit playerUnit;
    public abstract List<Vector2Int> GetAffectedTiles(Vector2Int position, Vector2Int direction);
    public abstract void ExecuteSpecial(Vector2Int position, Vector2Int direction, int damage);

    public void getGameBoardManager()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
    }

    public void getPlayerUnit()
    {
        playerUnit = FindObjectOfType<PlayerUnit>();
    }
}
