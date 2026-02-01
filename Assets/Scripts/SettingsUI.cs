using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Set slider values from saved volume
        musicSlider.value = SoundManager.instance.GetMusicVolume();
        sfxSlider.value = SoundManager.instance.GetSFXVolume();

        // Add listeners
        musicSlider.onValueChanged.AddListener(SetMusic);
        sfxSlider.onValueChanged.AddListener(SetSFX);
    }

    void SetMusic(float value)
    {
        SoundManager.instance.SetMusicVolume(value);
    }

    void SetSFX(float value)
    {
        SoundManager.instance.SetSFXVolume(value);
    }
}
