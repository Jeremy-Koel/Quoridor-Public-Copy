using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControllerMainMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource chalkWritingSource;
    public AudioSource chalkboardMovingSource;

    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("BackgroundVolume", Mathf.Log10(GameSession.BackgroundVolumePref) * 50);
        mixer.SetFloat("EffectsVolume", Mathf.Log10(GameSession.EffectVolumePref) * 50);

        chalkWritingSource = GameObject.Find("SoundEffectPlayer").GetComponent<AudioSource>();
        chalkboardMovingSource = GameObject.Find("SoundEffectPlayerChalkBoardMoving").GetComponent<AudioSource>();
    }

    public void PlayChalkWritingSound()
    {
        chalkWritingSource.Play();
    }
    public void PlayChalkboardMovingSound()
    {
        chalkboardMovingSource.Play();
    }

    public void StopChalkboardMovingSound()
    {
        chalkboardMovingSource.Stop();
    }
}
