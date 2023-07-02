using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hissing : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    private float cooldown;
    [SerializeField] float minCooldown = 4;
    [SerializeField] float maxCooldown = 8;
    private float timePassed = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cooldown = Random.Range(minCooldown, maxCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if (timePassed >= cooldown)
        {
            timePassed = 0;
            audioSource.Play();
            cooldown = Random.Range(minCooldown, maxCooldown);
        }
        else
            timePassed += Time.deltaTime;
    }
}
