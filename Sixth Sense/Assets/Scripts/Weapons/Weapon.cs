using UnityEngine;
using System.Collections.Generic;

public abstract class Weapon : ScriptableObject
{
    public string weaponName;
    public abstract List<Vector2Int> GetAttackTiles(Vector2Int position, Vector2Int direction);
}
