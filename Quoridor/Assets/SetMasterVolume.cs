using UnityEngine;
using UnityEngine.Audio;

public class SetMasterVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 50);
    }
}
