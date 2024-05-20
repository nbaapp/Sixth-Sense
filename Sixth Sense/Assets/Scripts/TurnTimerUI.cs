using UnityEngine;
using UnityEngine.UI;

public class TurnTimerUI : MonoBehaviour
{
    public Slider turnTimerProgressBar;
    public TurnManager turnManager;

    private void Update()
    {
        if (turnManager.isPlayerTurn)
        {
            UpdateProgressBar();
        }
    }

    private void UpdateProgressBar()
    {
        float remainingTime = turnManager.RemainingTurnTime;
        float maxTime = turnManager.CurrentTurnTime;
        turnTimerProgressBar.value = remainingTime / maxTime;
    }
}
