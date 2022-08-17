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
            masterVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
        }

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            audioMixer.SetFloat("effectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume")) * 20);
            effectsVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
            musicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("UserInterfaceVolume"))
        {
            audioMixer.SetFloat("userInterfaceVolume", Mathf.Log10(PlayerPrefs.GetFloat("UserInterfaceVolume")) * 20);
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
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetUserInterfaceVolume(float volume)
    {
        audioMixer.SetFloat("userInterfaceVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("UserInterfaceVolume", volume);
    }
}
