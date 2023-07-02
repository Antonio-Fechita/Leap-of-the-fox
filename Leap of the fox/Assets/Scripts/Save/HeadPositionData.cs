using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HeadPositionData
{
    public float[] headPosition = new float[3];

    public HeadPositionData(GameObject head)
    {
        headPosition[0] = head.transform.position.x;
        headPosition[1] = head.transform.position.y;
        headPosition[2] = head.transform.position.z;
    }
}
