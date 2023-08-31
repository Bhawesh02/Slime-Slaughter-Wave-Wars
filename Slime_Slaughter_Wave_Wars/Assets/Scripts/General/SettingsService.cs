
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class SettingsService : MonoBehaviour
{
    [SerializeField]
    private Slider bgVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private TMP_Dropdown resoltionDropdown;

    private Resolution[] resolutions;
    private void Awake()
    {
        backButton.onClick.AddListener(CloseThis);
        bgVolumeSlider.onValueChanged.AddListener(SetBgVol);
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVol);
        resoltionDropdown.onValueChanged.AddListener(SetResolution);


    }
    private void Start()
    {
        SetResolutionOptions();
    }

    private void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, UnityEngine.FullScreenMode.ExclusiveFullScreen, resolutions[resolutionIndex].refreshRateRatio);
    }

    private void SetResolutionOptions()
    {
        resolutions = Screen.resolutions;
        resoltionDropdown.ClearOptions();
        List<string> options = new();
        int currResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height +" @ " + resolutions[i].refreshRateRatio + "Hz");
            if (resolutions[i].Equals(Screen.currentResolution))
            {
                currResolutionIndex = i;
            }
        }
        resoltionDropdown.AddOptions(options);
        resoltionDropdown.value = currResolutionIndex;
        resoltionDropdown.RefreshShownValue();
    }

    private void SetSfxVol(float volume)
    {
        SoundService.Instance.SetSfxVolume(volume);
    }

    private void SetBgVol(float volume)
    {
        SoundService.Instance.SetBgVolume(volume);
    }

    

    private void CloseThis()
    {
        gameObject.SetActive(false);
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
    }
}
