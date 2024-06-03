using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider turnTimerProgressBar;
    private Image turnTimerFill;
    public TurnManager turnManager;
    public GameObject victoryScreen;
    public GameObject gameOverScreen;
    public GameObject standardUI;
    public Color feverColor;
    public Color normalColor;


    private void Start()
    {
        turnTimerFill = turnTimerProgressBar.fillRect.GetComponent<Image>();
    }

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
        gameOverScreen.SetActive(false);
        victoryScreen.SetActive(true);
    }

    public void ShowDefeatScreen()
    {
        standardUI.SetActive(false);
        victoryScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void SetFeverModeSlider(bool feverMode)
    {
        if (feverMode)
        {
           turnTimerFill.color = feverColor;
        }
        else
        {
            turnTimerFill.color = normalColor;
        }
    }
}
