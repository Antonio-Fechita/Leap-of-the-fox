using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 150;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
