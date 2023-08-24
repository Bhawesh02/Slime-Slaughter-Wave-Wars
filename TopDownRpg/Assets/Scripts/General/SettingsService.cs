
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
    private float initialBgVol = 0.03f;
    [SerializeField]
    private TMP_Dropdown resoltionDropdown;

    private Resolution[] resolutions;
    private void Awake()
    {
        backButton.onClick.AddListener(closeThis);
        bgVolumeSlider.onValueChanged.AddListener(setBgVol);
        sfxVolumeSlider.onValueChanged.AddListener(setSfxVol);
        resoltionDropdown.onValueChanged.AddListener(setResolution);


    }
    private void Start()
    {
        bgVolumeSlider.value = initialBgVol;
        setResolutionOptions();
    }

    private void setResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height,true);
    }

    private void setResolutionOptions()
    {
        resolutions = Screen.resolutions;
        resoltionDropdown.ClearOptions();
        List<string> options = new();
        int currResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height );
            if (resolutions[i].Equals(Screen.currentResolution))
            {
                currResolutionIndex = i;
            }
        }
        resoltionDropdown.AddOptions(options);
        resoltionDropdown.value = currResolutionIndex;
        resoltionDropdown.RefreshShownValue();
    }

    private void setSfxVol(float volume)
    {
        SoundService.Instance.SetSfxVolume(volume);
    }

    private void setBgVol(float volume)
    {
        SoundService.Instance.SetBgVolume(volume);
    }

    

    private void closeThis()
    {
        gameObject.SetActive(false);
        SoundService.Instance.PlaySfx(SoundService.Instance.Click);
    }
}
