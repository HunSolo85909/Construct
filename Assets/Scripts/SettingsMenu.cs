using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int index = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                index = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        if (!PlayerPrefs.HasKey("Resolution"))
            resolutionDropdown.value = index;
        else
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        resolutionDropdown.RefreshShownValue();

        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("Volume", -20f));
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen",1));
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        PlayerPrefs.SetInt("Quality", quality);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        if(fullscreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }

    public void SetResolution(int res)
    {
        Resolution resolution = resolutions[res];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", res);
    }
}
