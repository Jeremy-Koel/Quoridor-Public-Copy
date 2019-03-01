using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingController : MonoBehaviour
{
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        Slider masterVolumeSlider = GameObject.Find("MasterVolumeSlider").GetComponent<Slider>();
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);

        Slider backgroundVolumeSlider = GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>();
        backgroundVolumeSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1.0f);

        Slider effectsVolumeSlider = GameObject.Find("EffectsVolumeSlider").GetComponent<Slider>();
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
    }

    public void SetMasterLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", GetMixerValueFromSliderValue(sliderValue));
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetBackgroundLevel(float sliderValue)
    {
        mixer.SetFloat("BackgroundVolume", GetMixerValueFromSliderValue(sliderValue));
        PlayerPrefs.SetFloat("BackgroundVolume", sliderValue);
    }

    public void SetEffectsLevel(float sliderValue)
    {
        mixer.SetFloat("EffectsVolume", GetMixerValueFromSliderValue(sliderValue));
        PlayerPrefs.SetFloat("EffectsVolume", sliderValue);
    }

    private float GetMixerValueFromSliderValue(float f)
    {
        return Mathf.Log10(f) * 50;
    }
}
