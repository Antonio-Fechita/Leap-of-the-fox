using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectScaleCrushPoints : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localScale = PlayerController.instance.transform.localScale;       
    }
}
