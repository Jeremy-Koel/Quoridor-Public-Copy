using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControllerMainMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource chalkWritingSource;
    public AudioSource chalkboardMovingSource;

    private void Awake()
    {
        chalkWritingSource = GameObject.Find("SoundEffectPlayer").GetComponent<AudioSource>();
        chalkboardMovingSource = GameObject.Find("SoundEffectPlayerChalkBoardMoving").GetComponent<AudioSource>();

        chalkboardMovingSource.pitch = 1.35f;
        chalkboardMovingSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("BackgroundVolume", Mathf.Log10(GameSession.BackgroundVolumePref) * 50);
        mixer.SetFloat("EffectsVolume", Mathf.Log10(GameSession.EffectVolumePref) * 50);
    }

    public void PlayChalkWritingSound()
    {
        chalkWritingSource.Play();
    }
    public void PlayChalkboardMovingSound()
    {
        if (chalkboardMovingSource.pitch != 1f)
        {
            chalkboardMovingSource.pitch = 1f;
        }
        chalkboardMovingSource.Play();
    }

    public void StopChalkboardMovingSound()
    {
        chalkboardMovingSource.Stop();
    }
}
