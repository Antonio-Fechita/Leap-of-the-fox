using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GemsSaveData
{
    public int currentGems;

    public GemsSaveData(int gems)
    {
        currentGems = gems;
    }
}
