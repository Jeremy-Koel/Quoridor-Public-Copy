using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip errorClip;
    public AudioClip squeakClip;

    public void PlayWinSound()
    {
        source.clip = winClip;
        source.Play();
    }

    public void PlayLoseSound()
    {
        source.clip = loseClip;
        source.Play();
    }

    public void PlayErrorSound()
    {
        source.clip = errorClip;
        source.Play();
    }

    public void PlaySqueakSound()
    {
        source.clip = squeakClip;
        source.Play();
    }
}
