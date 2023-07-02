using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachutePickable : MonoBehaviour
{
    [SerializeField] GameObject parachute;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parachute.SetActive(true);
            Destroy(transform.gameObject);
        }
    }
}
