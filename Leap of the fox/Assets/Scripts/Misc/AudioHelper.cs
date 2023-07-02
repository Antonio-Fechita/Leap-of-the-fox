using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public static void CreateOneShotSound(AudioClip clip, Vector3 position)
    {
        GameObject soundObject = new GameObject("OneShotSound");
        soundObject.transform.position = position;

        AudioSource source = soundObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.playOnAwake = false;

        source.Play();

        Destroy(soundObject, clip.length);
    }
}
