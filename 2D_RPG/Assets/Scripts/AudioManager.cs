using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject inGameMasterVolumeSlider;
    public GameObject inGameEffectsVolumeSlider;
    public GameObject inGameMusicVolumeSlider;
    public GameObject inGameUserInterfaceVolumeSlider;

    public GameObject mainMenuMasterVolumeSlider;
    public GameObject mainMenuEffectsVolumeSlider;
    public GameObject mainMenuMusicVolumeSlider;
    public GameObject mainMenuUserInterfaceVolumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            audioMixer.SetFloat("masterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
            if (inGameMasterVolumeSlider != null)
                inGameMasterVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
            if (mainMenuMasterVolumeSlider != null)
                mainMenuMasterVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
        }

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            audioMixer.SetFloat("effectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume")) * 20);
            if (inGameEffectsVolumeSlider != null)
                inGameEffectsVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume");
            if (mainMenuEffectsVolumeSlider != null)
                mainMenuEffectsVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
            if (inGameMusicVolumeSlider != null)
                inGameMusicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
            if (mainMenuMusicVolumeSlider != null)
                mainMenuMusicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("UserInterfaceVolume"))
        {
            audioMixer.SetFloat("userInterfaceVolume", Mathf.Log10(PlayerPrefs.GetFloat("UserInterfaceVolume")) * 20);
            if (inGameUserInterfaceVolumeSlider != null)
                inGameUserInterfaceVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("UserInterfaceVolume");
            if (mainMenuUserInterfaceVolumeSlider != null)
                mainMenuUserInterfaceVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("UserInterfaceVolume");
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
        if(inGameMasterVolumeSlider != null)
            inGameMasterVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
        if(mainMenuMasterVolumeSlider != null)
            mainMenuMasterVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        if(inGameEffectsVolumeSlider != null)
            inGameEffectsVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume");
        if(mainMenuEffectsVolumeSlider != null)
            mainMenuEffectsVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume");
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        if(inGameMusicVolumeSlider != null)
            inGameMusicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        if(mainMenuMusicVolumeSlider != null)
            mainMenuMusicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetUserInterfaceVolume(float volume)
    {
        audioMixer.SetFloat("userInterfaceVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("UserInterfaceVolume", volume);
        if(inGameUserInterfaceVolumeSlider != null)
            inGameUserInterfaceVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("UserInterfaceVolume");
        if (mainMenuUserInterfaceVolumeSlider != null)
            mainMenuUserInterfaceVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("UserInterfaceVolume");
    }
}
