using UnityEngine;
using System.Collections.Generic;

public abstract class Weapon : ScriptableObject
{
    public string weaponName;
    public int physicalStrength = 1;
    public int magicalStrength = 0;
    public bool isPhysical = true;
    public bool isMagical = false;
    public abstract List<Vector2Int> GetAttackTiles(Vector2Int position, Vector2Int direction);
}
