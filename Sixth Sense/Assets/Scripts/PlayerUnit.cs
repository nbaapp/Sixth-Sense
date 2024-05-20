using UnityEngine;

public class PlayerUnit : Unit
{
    // Add any player-specific attributes or methods here

    protected override void Die()
    {
        base.Die();
        // Additional player-specific death handling
    }
}
