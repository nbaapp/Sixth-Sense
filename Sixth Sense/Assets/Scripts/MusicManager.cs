using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource battleMusic;
    public AudioSource victoryMusic;

    void Start()
    {
        battleMusic.Play();
    }

    public void PlayBattle()
    {
        victoryMusic.Stop();
        battleMusic.Play();
    }

    public void PlayVictory()
    {
        battleMusic.Stop();
        victoryMusic.Play();
    }
}
