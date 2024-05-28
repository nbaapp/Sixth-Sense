using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioClip battleMusic;
    public AudioClip victoryMusic;
    private bool isPlayingFirstClip = true;

    void Start()
    {
        PlayMusic(battleMusic);
    }

    void PlayMusic(AudioClip clip)
    {
        if (isPlayingFirstClip)
        {
            audioSource1.clip = clip;
            audioSource1.Play();
        }
        else
        {
            audioSource2.clip = clip;
            audioSource2.Play();
        }
    }

    public void SwitchMusic()
    {
        if (isPlayingFirstClip)
        {
            audioSource1.Stop();
            PlayMusic(victoryMusic);
        }
        else
        {
            audioSource2.Stop();
            PlayMusic(battleMusic);
        }
        isPlayingFirstClip = !isPlayingFirstClip;
    }
}
