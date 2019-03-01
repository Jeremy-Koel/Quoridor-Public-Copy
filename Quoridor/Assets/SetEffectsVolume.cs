using UnityEngine;
using UnityEngine.Audio;

public class SetEffectsVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(sliderValue) * 50);
    }
}
