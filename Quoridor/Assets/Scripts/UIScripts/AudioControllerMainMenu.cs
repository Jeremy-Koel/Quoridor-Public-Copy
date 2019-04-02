﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControllerMainMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource chalkWritingSource;

    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(GameSession.MasterVolumePref) * 50);
        mixer.SetFloat("BackgroundVolume", Mathf.Log10(GameSession.MasterVolumePref) * 50);
        mixer.SetFloat("EffectsVolume", Mathf.Log10(GameSession.MasterVolumePref) * 50);

        chalkWritingSource = GameObject.Find("SoundEffectPlayer").GetComponent<AudioSource>();
    }

    public void PlayChalkWritingSound()
    {
        chalkWritingSource.Play();
    }
    
}
