using UnityEngine;

public class EnemyUnit : Unit
{
    // Add any enemy-specific attributes or methods here

    protected override void Die()
    {
        base.Die();
        // Additional enemy-specific death handling
    }
}
