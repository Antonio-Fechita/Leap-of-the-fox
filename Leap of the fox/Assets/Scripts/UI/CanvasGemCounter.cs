using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasGemCounter : MonoBehaviour
{
    public TextMeshProUGUI textCounter;

    private void Start()
    {
        Collectables.gemCollectedEvent.AddListener(updateCounter);
    }

    private void updateCounter(int gems)
    {
        textCounter.text = gems.ToString() + " x";
    }
}
