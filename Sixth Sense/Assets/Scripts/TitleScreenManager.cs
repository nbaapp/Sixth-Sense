using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject mainTitle;
    public GameObject credits;
    public GameObject howToPlay;

    public void Start()
    {
        mainTitle.SetActive(true);
        credits.SetActive(false);
        howToPlay.SetActive(false);
    }

    public void OpenCredits()
    {
        mainTitle.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(true);
    }

    public void OpenHowToPlay()
    {
        mainTitle.SetActive(false);
        credits.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void OpenTitleScreen()
    {
        credits.SetActive(false);
        howToPlay.SetActive(false);
        mainTitle.SetActive(true);
    }
}
