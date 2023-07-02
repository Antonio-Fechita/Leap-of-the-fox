using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class VolumeData
{
    public float volumeMaster;
    public float volumeMusic;
    public float volumeSFX;

    public VolumeData(float volumeMaster, float volumeMusic, float volumeSFX)
    {
        this.volumeMaster = volumeMaster;
        this.volumeMusic = volumeMusic;
        this.volumeSFX = volumeSFX;
    }
}
