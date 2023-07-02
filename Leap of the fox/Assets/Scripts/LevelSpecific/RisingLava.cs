using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{
    Vector2 startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] Lever lever;
    [SerializeField] float speed = 5f;
    private bool activatedRising = false;


    private void Start()
    {
        lever.upEvent.AddListener(OnLeverActivated);
        startPoint = transform.position;
    }

    private void OnLeverActivated()
    {
        activatedRising = true;
    }

    private void Update()
    {
        if (activatedRising)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
        }
    }


}
