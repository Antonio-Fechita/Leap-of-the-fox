using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] ColorEnum color;
    private AudioSource audioSource;
    [SerializeField] AudioClip doorClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Key key = collision.transform.GetComponent<KeyManager>().GetKey(color);
            if(key != null)
            {
                AudioHelper.CreateOneShotSound(doorClip, transform.position);
                collision.transform.GetComponent<KeyManager>().RemoveKey(key);
                transform.gameObject.SetActive(false);
                Destroy(key.gameObject);
            }
        }
    }

}
