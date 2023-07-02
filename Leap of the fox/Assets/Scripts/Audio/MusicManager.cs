using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] float cooldownBetweenClips = 5f;
    public AudioMixer audioMixer;
    private bool inCooldown = false;
    private int songIndex;
    void Start()
    {
        LoadVolumes();
        audioSource = GetComponent<AudioSource>();
    }


    private AudioClip ChooseClip()
    {
        int newSongIndex = Random.Range(0, audioClips.Length - 1);
        while(newSongIndex == songIndex)
        {
            newSongIndex = Random.Range(0, audioClips.Length - 1);
        }
        songIndex = newSongIndex;

        return audioClips[songIndex];
    }

    IEnumerator ChangeSong()
    {

        audioSource.clip = ChooseClip();
        audioSource.Play();
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        yield return new WaitForSecondsRealtime(cooldownBetweenClips);
        inCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inCooldown)
        {
            inCooldown = true;
            StartCoroutine(ChangeSong());
        }
            
    }

    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSoundEffects(float volume)
    {
        audioMixer.SetFloat("soundEffectsVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMaster(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);

    }

    public void LoadVolumes()
    {
        VolumeData volumeData = SaveSystem.LoadVolume();
        SetMusic(volumeData.volumeMusic);
        SetSoundEffects(volumeData.volumeSFX);
        SetMaster(volumeData.volumeMaster);
        

    }


}
