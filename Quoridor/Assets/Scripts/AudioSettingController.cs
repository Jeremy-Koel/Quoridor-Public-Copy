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
        masterVolumeSlider.value = GameSession.MasterVolumePref;

        Slider backgroundVolumeSlider = GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>();
        backgroundVolumeSlider.value = GameSession.BackgroundVolumePref;

        Slider effectsVolumeSlider = GameObject.Find("EffectsVolumeSlider").GetComponent<Slider>();
        effectsVolumeSlider.value = GameSession.EffectVolumePref;
    }

    public void SetMasterLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", GetMixerValueFromSliderValue(sliderValue));
        GameSession.MasterVolumePref = sliderValue;
    }

    public void SetBackgroundLevel(float sliderValue)
    {
        mixer.SetFloat("BackgroundVolume", GetMixerValueFromSliderValue(sliderValue));
        GameSession.BackgroundVolumePref = sliderValue;
    }

    public void SetEffectsLevel(float sliderValue)
    {
        mixer.SetFloat("EffectsVolume", GetMixerValueFromSliderValue(sliderValue));
        GameSession.EffectVolumePref = sliderValue;
    }

    private float GetMixerValueFromSliderValue(float f)
    {
        return Mathf.Log10(f) * 50;
    }
}
