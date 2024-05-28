using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider turnTimerProgressBar;
    public TurnManager turnManager;
    public GameObject victoryScreen;
    public GameObject standardUI;

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

    public void ShowVictoryScreen()
    {
        standardUI.SetActive(false);
        victoryScreen.SetActive(true);
    }
}
