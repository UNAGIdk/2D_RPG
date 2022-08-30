using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject masterVolumeSlider;
    public GameObject effectsVolumeSlider;
    public GameObject musicVolumeSlider;
    public GameObject userInterfaceVolumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            audioMixer.SetFloat("masterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
            if (masterVolumeSlider != null)
                masterVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
        }

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            audioMixer.SetFloat("effectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume")) * 20);
            if (effectsVolumeSlider != null)
                effectsVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
            if (musicVolumeSlider != null)
                musicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("UserInterfaceVolume"))
        {
            audioMixer.SetFloat("userInterfaceVolume", Mathf.Log10(PlayerPrefs.GetFloat("UserInterfaceVolume")) * 20);
            if (userInterfaceVolumeSlider != null)
                userInterfaceVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("UserInterfaceVolume");
        }


        //вернуть на старте значения громкости на 0 Db
        /*
        audioMixer.SetFloat("masterVolume", Mathf.Log10(1) * 20);
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(1) * 20);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(1) * 20);
        audioMixer.SetFloat("userInterfaceVolume", Mathf.Log10(1) * 20);
        */
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20); //структура с логарифмом нужна потому что в микшере мин. громкость это -80, а макс. громкость это 0
        PlayerPrefs.SetFloat("MasterVolume", volume);
        masterVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        effectsVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume");
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        musicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetUserInterfaceVolume(float volume)
    {
        audioMixer.SetFloat("userInterfaceVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("UserInterfaceVolume", volume);
        userInterfaceVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("UserInterfaceVolume");
    }
}
