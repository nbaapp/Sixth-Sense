using UnityEngine;
using System.Collections.Generic;

public abstract class Special : ScriptableObject
{
    public string specialName;
    public int cooldown;
    public abstract List<Vector2Int> GetAffectedTiles(Vector2Int position, Vector2Int direction);
    public abstract void ExecuteSpecial(Vector2Int position, Vector2Int direction);
}
