using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public UIManager uiManager;
    public PlayerController playerController;
    public MusicManager musicManager;
    
    public void Victory()
    {
        uiManager.ShowVictoryScreen();
        playerController.DisableControls();
        musicManager.SwitchMusic();
    }
    
}
