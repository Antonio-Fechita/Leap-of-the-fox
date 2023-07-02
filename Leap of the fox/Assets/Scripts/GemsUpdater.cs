using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsUpdater : MonoBehaviour
{
    
    void Start()
    {
        Debug.Log("Gems collected overall: " + SaveSystem.LoadGems().currentGems);
    }


}
