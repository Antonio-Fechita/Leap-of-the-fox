using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    [SerializeField]
    Sprite attackingSprite;
    SpriteRenderer spriteRenderer;
    [SerializeField] int damageValue;
    [SerializeField] float cooldown;
    private bool inCooldown = false;
    [SerializeField] AudioClip axeAttackClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(spriteRenderer.sprite == attackingSprite && !inCooldown)
        {
            audioSource.clip = axeAttackClip;
            audioSource.Play();
            inCooldown = true;
            StartCoroutine(cooldownTimer());

            if (PlayerController.instance.GetComponent<Shielding>().bubbleShield.active)
            {
                PlayerController.instance.GetComponent<Shielding>().BreakShield(5);
            }
            else
            {
                PlayerHealthController.instance.takeDamage(damageValue, Mathf.Sign(transform.localScale.x) * 10, 0);
            }

        }
    }

    IEnumerator cooldownTimer()
    {
        yield return new WaitForSeconds(cooldown);
        inCooldown = false;
    }
}
