using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Image blackScreen;
    public float fadeDuration = 2.0f;

    private bool hasPlayed = false;
    private float fadeTimer = 0f;
    private Color blackColor = Color.black;
    private Color clearColor = Color.clear;

    void Start()
    {
        blackScreen.color = blackColor;
        audioSource.Play();
    }

    void Update()
    {
        if (!hasPlayed && !audioSource.isPlaying)
        {
            hasPlayed = true;
            fadeTimer = 0f;
        }

        if (hasPlayed)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);

            if (fadeTimer >= fadeDuration)
            {
                SceneManager.LoadScene("TitleScreen");
            }
        }
    }
}
