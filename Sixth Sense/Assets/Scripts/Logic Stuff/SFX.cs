using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource impact;
    public AudioSource swordWhoosh;
    public AudioSource ugh;

    public void PlayImpact()
    {
        impact.Play();
    }

    public void PlaySwordWhoosh()
    {
        swordWhoosh.Play();
    }

    public void PlayUgh()
    {
        ugh.Play();
    }
}
