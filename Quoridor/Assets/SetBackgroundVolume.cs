using UnityEngine;
using UnityEngine.Audio;

public class SetBackgroundVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("BackgroundVolume", Mathf.Log10(sliderValue) * 50);
    }
}
