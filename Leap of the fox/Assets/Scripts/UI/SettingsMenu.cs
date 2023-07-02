using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    public Slider master;
    public Slider music;
    public Slider soundEffects;


    public AudioMixer audioMixer;

    public float masterVolume;
    public float musicVolume;
    public float soundEffectsVolume;
    public static SettingsMenu instance;


    private Resolution[] resolutions;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();
        int defaultResolutionIndex = 0;

        List<string> options = new List<string>();

        for(int index = 0; index < resolutions.Length; index++)
        {
            string option = resolutions[index].width + " x " + resolutions[index].height + " @ " + resolutions[index].refreshRate + "hz";
            options.Add(option);

            if(resolutions[index].width == Screen.currentResolution.width && resolutions[index].height == Screen.currentResolution.height 
                && resolutions[index].refreshRate == Screen.currentResolution.refreshRate)
            {
                defaultResolutionIndex = index;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = defaultResolutionIndex;
        resolutionsDropdown.RefreshShownValue();

        VolumeData volumeData = SaveSystem.LoadVolume();
        master.value = volumeData.volumeMaster;
        soundEffects.value = volumeData.volumeSFX;
        music.value = volumeData.volumeMusic;
    }

    public void SetMusic(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        Debug.Log("Music: " + volume);
    }

    public void SetSoundEffects(float volume)
    {
        soundEffectsVolume = volume;
        audioMixer.SetFloat("soundEffectsVolume", Mathf.Log10(volume) * 20);
        Debug.Log("Sound Effects: " + volume);
    }

    public void SetMaster(float volume)
    {
        masterVolume = volume;
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        Debug.Log("Master: " + volume);

    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("Setted quality index to " + qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SaveVolumes()
    {
        SaveSystem.SaveVolume(new VolumeData(masterVolume, musicVolume, soundEffectsVolume));
    }



}
