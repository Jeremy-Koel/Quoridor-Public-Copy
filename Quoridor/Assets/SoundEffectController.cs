using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public AudioSource gameOverSoundSource;
    public AudioClip winClip;
    public AudioClip loseClip;

    public void PlayWinSound()
    {
        gameOverSoundSource.clip = winClip;
        gameOverSoundSource.Play();
    }

    public void PlayLoseSound()
    {
        gameOverSoundSource.clip = loseClip;
        gameOverSoundSource.Play();
    }

}
