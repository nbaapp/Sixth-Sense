using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource battleMusic;
    public AudioSource victoryMusic;
    public AudioSource gameOverMusic;

    void Start()
    {
        battleMusic.Play();
    }

    public void PlayBattle()
    {
        StopMusic();
        battleMusic.Play();
    }

    public void PlayVictory()
    {
        StopMusic();
        victoryMusic.Play();
    }

    public void PlayGameOver()
    {
        StopMusic();
        gameOverMusic.Play();
    }

    public void StopMusic()
    {
        battleMusic.Stop();
        victoryMusic.Stop();
        gameOverMusic.Stop();
    }
}
